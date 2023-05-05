using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CashierDB;
using CashierDB.Tables;
using CashierUI.Dto;
using CashierUI.Helper;
using CashierUI.Parts;
using CashierUI.Parts.AddSystems;
using CashierUI.Parts.EditSystems;
using Microsoft.EntityFrameworkCore;

namespace CashierUI.ViewModels
{
    public class MainTabViewModel : INotifyPropertyChanged
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
        private CashierContext _context;
        public MainTabViewModel(CashierContext context)
        {
            _context = context;           
        }

        #region Menu  
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
                var userControl = new MenuTab();
                userControl.DataContext = new MenuTabViewModel(_context, this, tab.Name);
                tabItem.Content = userControl;
                tabItem.Header = tab.Name;
                tabItem.FontSize = 20;
                tabItem.Height = 35;
                tabItem.Width = 100;
                MenuTabs.Add(tabItem);
            }
        }        
        public UserControl MenuTabUC { get; set; }      
        public void propertyChangeInMenu()
        {
            OnPropertyChanged(nameof(MenuTabUC));      
        }
        #endregion
        #region Tabs
        public ObservableCollection<OpenTabDetails> OpenTabs { get; set; } = new();
        public void LoadOpenTabs()
        {
            var tabs = _context.Tabs
                .Include(c=>c.OrderLists)
                .Where(c=>c.IsClose == false)
                .OrderBy(c=>c.Date)
                .Select(c=> new OpenTabDetails(c))
                .ToList();
            OpenTabs.Clear();
            foreach (var tab in tabs) OpenTabs.Add(tab);
        }
        public AddTabViewModel addTabVM { get; set; }
        public UserControl TabUC { get; set; }
        public void AddTab()
        {
            addTabVM = new AddTabViewModel(_context, this);
            TabUC = new AddTab();
            TabUC.DataContext = addTabVM;
            OnPropertyChanged(nameof(TabUC));
        }
        public void CheckAddTab()
        {
            if (addTabVM.DialogResult)
            {
                TabUC = null;
                LoadOpenTabs();
            }
            OnPropertyChanged(nameof(TabUC));
        }
        public EditTabViewModel editTabVM { get; set; }
        public void EditTab(OpenTabDetails tab)
        {
            editTabVM = new EditTabViewModel(_context, this, tab);
            TabUC = new EditTab();
            TabUC.DataContext = editTabVM;
            OnPropertyChanged(nameof(TabUC));
        }
        public void CheckEditTab()
        {
            if (editTabVM.DialogResult)
            {
                TabUC = null;
                LoadOpenTabs();
            }
            OnPropertyChanged(nameof(TabUC));
        }
        public void PayTab(OpenTabDetails tab)
        {
            var viewmodel = new PayWindowViewModel(_context,this, tab);
            var window = new PayWindow();
            window.DataContext = viewmodel;
            window.ShowDialog();
        }
        public void CloseTab(OpenTabDetails tabToClose)
        {
            if (tabToClose.PaymentStatus != "Paid")
            {
                MessageBox.Show("You cannot close an unpaid tab", "Error");
                return;
            }
            var tab = _context.Tabs.First(c => c.TabId == tabToClose.TabId);
            tab.IsClose = true;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
            LoadOpenTabs() ;
        }
        public void RemoveTab(OpenTabDetails tabToRemove)
        {
            if (tabToRemove.PaymentStatus == "Paid")
            {
                MessageBox.Show("You cannot remove an paid tab", "Error");
                return;
            }
            var tab = _context.Tabs.First(c => c.TabId == tabToRemove.TabId);
            tab.OrderLists.Clear();
            try
            {
                _context.Tabs.Remove(tab);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
            LoadOpenTabs();
        }
        public void Serve(ShortOrderItem item)
        {
            var menuItem = _context.MenuItems.First(c => c.MenuItemId == item.MenuItemId);
            if (item.IsServed) menuItem.Stock -= item.Quantity;          
            else menuItem.Stock += item.Quantity;
            try
            {
                _context.SaveChanges();
                LoadMenuTabs();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
        #endregion
        #region TabsRecords
        public ReceiptRecordsViewModel receiptRecordsVM { get; set; }
        public void ViewTabs()
        {
            receiptRecordsVM = new ReceiptRecordsViewModel(_context,this);
            TabUC = new ReceiptRecordsView();
            TabUC.DataContext = receiptRecordsVM;
            OnPropertyChanged(nameof(TabUC));
        }
        public void CheckViewTabs()
        {
            if (receiptRecordsVM.DialogResult) TabUC = null;
            OnPropertyChanged(nameof(TabUC));
        }
        #endregion
    }
}
