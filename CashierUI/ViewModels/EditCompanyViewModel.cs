using CashierDB;
using CashierUI.Dto;
using CashierUI.Parts.AddSystems;
using FluentValidation;
using FluentValidation.Results;
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

namespace CashierUI.ViewModels
{
    public class EditCompanyViewModel :INotifyPropertyChanged
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
        public CashierContext _context { get; set; }
        public MainTabViewModel Parent { get; set; }
        public EditCompanyViewModel(CashierContext context, MainTabViewModel parent)
        {
            _context = context;
            Parent = parent;
            CompanyName = parent.CompanyName;
            Address = parent.Address;
            PriceModifiers = parent.PriceModifiers;
        }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public ObservableCollection<PriceModifiersName> PriceModifiers { get; set; } = new();
        public ObservableCollection<string> Errors { get; set; } = new();
        public void Add()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var company = _context.Company.First();
                company.Name = CompanyName;
                company.Address = Address;
                try
                {
                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }
            Parent.CheckEditComp();
        }
        public void Cancel()
        {
            DialogResult = true;
            Parent.CheckEditComp();
        }
        public void Remove (PriceModifiersName pmToRemove)
        {
            var pm = _context.PriceModifiers.First(c => c.PriceModifierId == pmToRemove.PriceModifiersID);
            if (pm.Tabs != null) pm.Tabs.Clear();
            try
            {
                _context.PriceModifiers.Remove(pm);
                _context.SaveChanges();
                LoadPriceMods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
        public bool DialogResult { get; set; }
        public void ActivateAutoApply(PriceModifiersName pm)
        {
            var priceModifier = _context.PriceModifiers.First(c=>c.PriceModifierId == pm.PriceModifiersID);
            if (pm.boolAutoApply)
            {
                priceModifier.AutoApply = true;
            }
            else priceModifier.AutoApply = false;
            try
            {
                _context.SaveChanges();
                LoadPriceMods();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }

        }
        public void LoadPriceMods()
        {
            var priceMods = _context.PriceModifiers.Select(c => new PriceModifiersName(c.PriceModifierId, c.Name, c.Percentage, c.IsAdd, c.AutoApply)).ToList();
            PriceModifiers.Clear();
            foreach (var mod in priceMods) PriceModifiers.Add(mod);
        }
        public virtual bool Validate()
        {
            var validator = new EditCompanyValidator();
            var result = validator.Validate(this);
            Errors.Clear();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
            return result.IsValid;
        }
        #region PriceModifiers
        public AddPriceModifierViewModel priceModifierVM { get; set; }
        public UserControl AddPriceModUC { get; set; }
        public void AddPM()
        {
            priceModifierVM = new AddPriceModifierViewModel(_context,this);
            AddPriceModUC = new AddPriceModifier();
            AddPriceModUC.DataContext = priceModifierVM;
            OnPropertyChanged(nameof(AddPriceModUC));
        }
        public void CheckAddPM()
        {
            if (priceModifierVM.DialogResult)
            {
                AddPriceModUC = null;
                LoadPriceMods();
            }
            OnPropertyChanged(nameof(AddPriceModUC));
        }
        #endregion
    }
    public class EditCompanyValidator : AbstractValidator<EditCompanyViewModel>
    {
        public EditCompanyValidator()
        {
            RuleFor(c => c.CompanyName).NotEmpty();
            RuleFor(c => c.Address).NotEmpty();         
        }
    }
}
