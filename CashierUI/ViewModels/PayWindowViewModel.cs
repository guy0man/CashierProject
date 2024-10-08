﻿using CashierDB;
using CashierUI.Dto;
using CashierUI.Parts.EditSystems;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CashierUI.ViewModels
{
    public class PayWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
        public CashierContext _context {  get; set; }
        public OpenTabDetails Tab { get; set; }
        public MainTabViewModel Parent { get; set; }
        public PayWindowViewModel (CashierContext context, MainTabViewModel parent,  OpenTabDetails tab)
        {
            _context = context;
            Tab = tab;
            Parent = parent;
            Invoice = Tab.Total;
            OriginalInvoice = float.Parse(Invoice.Remove(0,1));
            numInvoice = OriginalInvoice;
            PaymentAmount = string.Empty;
            LoadPriceModifiers();
            Tip = string.Empty;           
        }
        public ObservableCollection<PriceModifiedView> PriceModifiers { get; set; } = new();
        public void LoadPriceModifiers()
        {
            var priceModifiers = _context.PriceModifiersApplications
                .Include(c=>c.PriceModifierLink)
                .Where(c=>c.TabId == Tab.TabId)
                .Select(c=> new PriceModifiedView(c))
                .ToList();
            PriceModifiers.Clear();
            foreach(var mod in priceModifiers) PriceModifiers.Add(mod);
        }
        public ObservableCollection<string> Errors { get; set; } = new();
        public string _tip;
        public string Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                OnPropertyChanged();
                LoadNewInvoice();
                CalculateChange();
            }
        }
        public float OriginalInvoice { get; set; }
        public float numInvoice { get; set; }
        public string Invoice { get; set; }
        public string _paymentAmount;
        public string PaymentAmount
        {
            get => _paymentAmount;
            set
            {
                _paymentAmount = value;
                OnPropertyChanged();
                CalculateChange();
            }
        }
        public float numChange { get; set; }
        public string Change { get; set; }
        public float GetPriceModSum()
        {
            float result = 0;
            foreach (var mod in PriceModifiers)
            {
                if (mod.IsAdd)
                {
                    float addition = float.Parse(mod.Total.Remove(0, 1));
                    result += addition;                   
                }
                else
                {
                    float subtraction = float.Parse(mod.Total.Remove(0, 1));
                    result -= subtraction;
                }
            }
            return result;
        }
        public void LoadNewInvoice()
        {
            var PriceMod = GetPriceModSum();
            if (_tip == "0" || _tip == string.Empty)
            {               
                numInvoice = OriginalInvoice + PriceMod;
                Invoice = $"₱{numInvoice:N2}";
                OnPropertyChanged(nameof(Invoice));
                return;
            }
            numInvoice = OriginalInvoice + PriceMod;
            var tip = float.Parse(_tip);
            var newInvoice = tip + numInvoice;
            numInvoice = newInvoice;
            Invoice = $"₱{newInvoice:N2}";
            OnPropertyChanged(nameof(Invoice));
        }
        public void CalculateChange()
        {
            float paymentAmount;
            if (_paymentAmount == string.Empty) paymentAmount = 0;          
            else paymentAmount = float.Parse(_paymentAmount);
            var change = paymentAmount - numInvoice;
            numChange = change;
            Change = $"₱{change:N2}";
            OnPropertyChanged(nameof(Change));
        }
        public virtual bool Validate()
        {
            var validator = new PayWindowValidator();
            var result = validator.Validate(this);
            Errors.Clear();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
            return result.IsValid;
        }
        public void Pay()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var tab = _context.Tabs.First(c => c.TabId == Tab.TabId);
                if (Tip != string.Empty && Tip != "0") tab.Tip = float.Parse(Tip);
                else tab.Tip = 0;
                tab.Change = float.Parse(Change.Remove(0,1));
                tab.IsPaid = true;
                try
                {
                    _context.SaveChanges();
                    Parent.LoadOpenTabs();
                    DialogResult = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    DialogResult = false;
                }
            }
        }
        public EditAppliedPriceModifiersViewModel editPriceModifierVM { get; set; }
        public UserControl userControl { get; set; }
        public void EditPM()
        {
            editPriceModifierVM = new EditAppliedPriceModifiersViewModel(_context,this);
            userControl = new EditAppliedPriceModifier();
            userControl.DataContext = editPriceModifierVM;
            OnPropertyChanged(nameof(userControl));
        }
        public void CheckEditPM()
        {
            if (editPriceModifierVM.DialogResult)
            {
                userControl = null;
                LoadPriceModifiers();
                LoadNewInvoice();   
            }
            OnPropertyChanged(nameof(userControl));
        }

        public bool DialogResult { get; set; }
    }
    public class PayWindowValidator : AbstractValidator<PayWindowViewModel>
    {
        public PayWindowValidator()
        {
            RuleFor(c => c.PaymentAmount).NotEmpty();
            RuleFor(c => c.numChange).Must(c => c >= 0).WithMessage("Change must not be negative");
        }
    }

}
