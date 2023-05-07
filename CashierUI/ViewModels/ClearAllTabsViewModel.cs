using CashierDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierUI.ViewModels
{
    public class ClearAllTabsViewModel : INotifyPropertyChanged
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
        public ClearAllTabsViewModel(CashierContext context, MainTabViewModel parent)
        {
            _context = context;
            Parent = parent;
            WarningDescription = $"To confirm the permanent deletion of all TABS type \"DELETE\" in the input box";
            confirmationDELETE = string.Empty;
        }
        public string WarningDescription { get; set; }
        public string confirmationDELETE { get; set; }
        public string Error { get; set; }
        public bool Validate()
        {
            bool result;
            if (confirmationDELETE != "DELETE")
            {
                Error = "Invalid input";
                OnPropertyChanged(nameof(Error));
                result = false;
            }
            else result = true;
            return result;
        }
        public virtual void Confirm()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var tabs = _context.Tabs.Include(c => c.OrderLists);
                if (tabs.Count() == 0) return;
                try
                {
                    foreach (var tab in tabs)
                    {
                        if (tab.OrderLists != null) tab.OrderLists.Clear();
                        _context.Remove(tab);
                    }
                    _context.SaveChanges();
                    Parent.LoadOpenTabs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }
        }
    }
}
