using CashierDB;
using CashierUI.Dto;
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

namespace CashierUI.ViewModels
{
    public class ViewMenuViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

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
        public AddTabViewModel Parent { get; set; }
        public string MenuType { get; set; }
        public ViewMenuViewModel(CashierContext context, AddTabViewModel parent, string menuType)
        {
            _context = context;
            Parent = parent;
            MenuType = menuType.ToString();
            LoadMenuItems();
        }
        public ObservableCollection<MenuItemName> MenuItems { get; set; } = new();
        public void LoadMenuItems()
        {
            var items = _context.MenuItems
                .Include(c=>c.ItemTypeLink)
                .Where(c => c.ItemTypeLink.Name == MenuType)
                .OrderBy(c => c.Name)
                .Select(c => new MenuItemName(c))
                .ToList();
            MenuItems.Clear();
            foreach (var item in items) MenuItems.Add(item);
        }
        public void AddOrder(MenuItemName item)
        {
            if (item.Stock == 0)
            {
                MessageBox.Show("This item is out of stock", "Error");
                return;
            }
            var menuItem = _context.MenuItems.First(c => c.MenuItemId == item.MenuItemId);
            menuItem.Stock -= 1;
            var order = new PartialOrderItem(item);
            try
            {
                _context.SaveChanges();
                Parent.Orders.Add(order);
                Parent.LoadTotal();
                LoadMenuItems();
            }      
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }             
        }
    }
}
