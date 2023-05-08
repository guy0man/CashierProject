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
using CashierUI.Parts.ClearSystems;
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
            if (MenuTabs.Count == 0) addMenuVis = Visibility.Visible;
            else addMenuVis = Visibility.Collapsed;
            OnPropertyChanged(nameof(addMenuVis));
        }
        public Visibility addMenuVis { get; set; }
        public MenuTabViewModel menuVM { get; set; }
        public void AddMenu()
        {
            menuVM = new MenuTabViewModel(_context, this, "Helper");
            menuVM.AddMenuItem();
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
        public void CheckCancelEditTab()
        {
            if (editTabVM.DialogResult)
            {
                TabUC = null;
            }
            OnPropertyChanged(nameof(TabUC));
        }
        public void PayTab(OpenTabDetails tab)
        {
            if (tab._paymentStatus == "Paid")
            {
                MessageBox.Show("This tab is already paid");
                return;
            }
            var viewmodel = new PayWindowViewModel(_context,this, tab);
            var window = new PayWindow();
            window.DataContext = viewmodel;
            window.ShowDialog();
        }
        public void CloseTab(OpenTabDetails tabToClose)
        {
            if (tabToClose.Orders.Any(c=>c.IsServed == false))
            {
                MessageBox.Show("You cannot close a tab with unserved orders", "Errors");
                return;
            }
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
        public void ServeOrder(ShortOrderItem itemToServe)
        {
            var order = _context.OrderLists.First(c => c.OrderListId == itemToServe.OrderItemId);
            if(itemToServe.IsServed) order.IsServed = false;         
            else order.IsServed = true;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
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
        #region Settings
        public UserControl SettingsUC { get;set; }
        public void OpenSettings()
        {
            SettingsUC = new Settings();
            OnPropertyChanged(nameof(SettingsUC));
        }
        public void CloseSettings()
        {
            SettingsUC = null;
            OnPropertyChanged(nameof(SettingsUC));
        }
        public void ClearTabs()
        {
            var datacontext = new ClearAllTabsViewModel(_context, this);
            var window = new ConfirmClearTabs();
            window.DataContext = datacontext;
            window.ShowDialog();
        }
        public void ClearMenu()
        {
            var datacontext = new ClearMenuViewModel(_context, this);
            var window = new ConfirmClearMenu();
            window.DataContext = datacontext;
            window.ShowDialog();

        }
        public void ClearDatabase()
        {
            var datacontext = new ClearDatabaseViewModel(_context, this);
            var window = new ConfirmClearDatabase();
            window.DataContext = datacontext;
            window.ShowDialog();
        }
        #endregion
        #region Company
        public ObservableCollection<PriceModifiersName> PriceModifiers { get; set; } = new();
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public void LoadCompany()
        {
            var company = _context.Company.First();
            if (company.Name != null) CompanyName = company.Name;
            if (company.Address != null) Address = company.Address;          
        }
        public void LoadPriceMods()
        {
            var priceMods = _context.PriceModifiers.Select(c => new PriceModifiersName(c.PriceModifierId, c.Name, c.Percentage, c.IsAdd)).ToList();
            PriceModifiers.Clear();
            foreach (var mod in priceMods) PriceModifiers.Add(mod);
        }
        public EditCompanyViewModel editCompVM { get; set; }
        public UserControl CompanyUC { get; set; }
        public void EditCompany()
        {
            editCompVM = new EditCompanyViewModel(_context, this);
            CompanyUC = new EditCompany();
            CompanyUC.DataContext = editCompVM;
            OnPropertyChanged(nameof(CompanyUC));
        }
        public void CheckEditComp()
        {
            if (editCompVM.DialogResult)
            {
                CompanyUC = null;
                LoadCompany();
                LoadPriceMods();
            }
            OnPropertyChanged(nameof(CompanyUC));
            OnPropertyChanged(nameof(CompanyName));
            OnPropertyChanged(nameof(Address));
        }
        #endregion
    }
}
