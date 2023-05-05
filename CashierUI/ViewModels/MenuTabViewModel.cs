using CashierDB;
using CashierUI.Dto;
using CashierUI.Parts.EditSystems;
using CashierUI.Parts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CashierUI.Parts.AddSystems;

namespace CashierUI.ViewModels
{
    public class MenuTabViewModel : INotifyPropertyChanged
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
        public MainTabViewModel Parent { get; set; }
        public string MenuType { get; set; }
        public MenuTabViewModel(CashierContext context, MainTabViewModel parent, string menuType)
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
                .Where(c=>c.ItemTypeLink.Name == MenuType)
                .OrderBy(c=>c.Name)
                .Select(c=> new MenuItemName(c))
                .ToList();
            MenuItems.Clear();
            foreach (var item in items) MenuItems.Add(item);
        }
        public void RemoveMenuItem(MenuItemName itemToDelete)
        {
            var item = _context.MenuItems.First(c => c.MenuItemId == itemToDelete.MenuItemId);
            if (item.OrderLists != null) item.OrderLists.Clear();
            _context.MenuItems.Remove(item);
            try
            {
                _context.SaveChanges();
                LoadMenuItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
        public AddMenuItemViewModel addMenuVM { get; set; }
        public void AddMenuCheck()
        {
            if (addMenuVM.DialogResult)
            {
                Parent.MenuTabUC = null;
                Parent.LoadMenuTabs();
            }
            Parent.propertyChangeInMenu();

        }
        public void AddMenuItem()
        {
            addMenuVM = new AddMenuItemViewModel(_context, this);
            Parent.MenuTabUC = new AddMenuItem();
            Parent.MenuTabUC.DataContext = addMenuVM;
            Parent.propertyChangeInMenu();
        }
        public EditMenuItemViewModel editMenuVM { get; set; }
        public void EditMenuCheck()
        {
            if (editMenuVM.DialogResult)
            {
                Parent.MenuTabUC = null;
                LoadMenuItems();
            }
            Parent.propertyChangeInMenu();
        }
        public void EditMenuItem(MenuItemName itemToEdit)
        {
            editMenuVM = new EditMenuItemViewModel(_context, itemToEdit,this);
            Parent.MenuTabUC = new EditMenuItem();
            Parent.MenuTabUC.DataContext = editMenuVM;
            Parent.propertyChangeInMenu();
        }
    }
}
