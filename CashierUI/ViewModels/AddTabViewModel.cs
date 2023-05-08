using CashierDB;
using CashierDB.Tables;
using CashierUI.Dto;
using CashierUI.Parts;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CashierUI.ViewModels
{
    public class AddTabViewModel : INotifyPropertyChanged
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
        public AddTabViewModel(CashierContext context, MainTabViewModel parent)
        {
            _context = context;
            Parent = parent;
            LoadMenuTabs();
        }
        public ObservableCollection<TabItem> MenuTabs { get; set; } = new();
        public void LoadMenuTabs()
        {
            var menuTabs = _context.ItemTypes
                .OrderBy(c => c.Name)
                .Select(c => new ItemTypeName(c.ItemTypeId, c.Name))
                .ToList();
            MenuTabs.Clear();
            foreach (var tab in menuTabs)
            {
                var tabItem = new TabItem();
                var userControl = new MenuViewTab();
                userControl.DataContext = new ViewMenuViewModel(_context, this, tab.Name);
                tabItem.Content = userControl;
                tabItem.Header = tab.Name;
                tabItem.FontSize = 20;
                tabItem.Height = 35;
                tabItem.Width = 100;
                MenuTabs.Add(tabItem);
            }
        }
        public ObservableCollection<PartialOrderItem> Orders { get; set; } = new();       
        public ObservableCollection<string> Errors { get; set; } = new();
        public string Name { get; set; }
        public string TabTotal { get; set; }
        public bool IsTakeOut { get; set; }         
        public void LoadTotal()
        {
            string total = $"₱{Orders.Sum(c => c.RealTotal)}";
            TabTotal = total;
            OnPropertyChanged(nameof(TabTotal));
        }
        public void CalculateTotal(int num, PartialOrderItem order)
        {
            var total = order.Price * num;
            order.Total = total;
            if (order.IsCanceled != true)
            {
                order.RealTotal = total;
                order.realTotalText = $"₱{total}";
            }

        }
        public void RemoveOrder(PartialOrderItem order)
        {         
            var menuItem = _context.MenuItems.First(c=>c.MenuItemId == order.MenuItemId);
            menuItem.Stock += order.Quantity;
            try
            {
                _context.SaveChanges();
                Orders.Remove(order);
                LoadTotal();
                LoadMenuTabs();
            }     
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
        public void AddQuant(PartialOrderItem order)
        {
            var menuItem = _context.MenuItems.First(c=>c.MenuItemId==order.MenuItemId);
            if (menuItem.Stock == 0)
            {
                MessageBox.Show("This item is out of stock", "Error");
                return;
            }
            else menuItem.Stock -= 1;
            try
            {
                _context.SaveChanges();
                order.Quantity++;
                CalculateTotal(order.Quantity, order);
                OnPropertyChanged(nameof(Orders));
                LoadTotal();
                LoadMenuTabs();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
        public void MinusQuant(PartialOrderItem order)
        {
            if (order.ServedQuantity != 0)
            {
                if (order.Quantity == order.ServedQuantity) return;
            }
            else if (order.Quantity == 1) return;
            var menuItem = _context.MenuItems.First(c => c.MenuItemId == order.MenuItemId);
            menuItem.Stock += 1;
            try
            {
                _context.SaveChanges();
                order.Quantity--;
                CalculateTotal(order.Quantity, order);
                LoadTotal();
                LoadMenuTabs();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
        public virtual void Add()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var tab = new Tab();
                tab.CustomerName = Name;
                tab.CompanyId = 1;
                tab.Date = DateTime.Now;
                tab.Total = float.Parse(TabTotal.Remove(0,1));
                tab.Tip = 0;
                tab.IsClose = false;
                tab.IsPaid = false;
                tab.IsTakeOut = IsTakeOut;
                tab.OrderLists = new List<OrderList>();
                foreach (var order in Orders)
                {
                    tab.OrderLists.Add(new OrderList()
                    {
                        MenuItemId = order.MenuItemId,
                        Quantity = order.Quantity,
                        Total = order.Total,
                        RealTotal = order.RealTotal,
                        IsCanceled = order.IsCanceled,
                        IsServed = order.IsServed,
                        IsOld = order.IsOld
                    });
                }
                try
                {
                    _context.Add(tab);
                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }

            }
            Parent.CheckAddTab();
        }
        public virtual void Cancel()
        {
            foreach (var order in Orders)
            {
                var menuitem = _context.MenuItems.First(c=>c.MenuItemId == order.MenuItemId);
                menuitem.Stock += order.Quantity;
            }
            try
            {
                _context.SaveChanges();
                DialogResult = true;
            }  
            catch(Exception ex) 
            {
                DialogResult = false;
                MessageBox.Show(ex.InnerException.Message); 
            }
            Parent.CheckAddTab();
        }
        
        public bool DialogResult { get; set; }
        public virtual bool Validate()
        {
            var validator = new AddTabValidator();
            var result = validator.Validate(this);
            Errors.Clear();
            foreach (var error in result.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
            return result.IsValid;
        }
    }
    public class AddTabValidator : AbstractValidator<AddTabViewModel>
    {
        public AddTabValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c=>c.Orders).NotEmpty();
        }
    }
}
