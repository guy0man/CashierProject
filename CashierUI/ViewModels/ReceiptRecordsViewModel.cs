using CashierDB;
using CashierUI.Dto;
using CashierUI.Helper;
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

namespace CashierUI.ViewModels
{
    public class ReceiptRecordsViewModel : INotifyPropertyChanged
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
        public Pagination PageDetails { get; private set; }
        public ReceiptRecordsViewModel(CashierContext context, MainTabViewModel parent)
        {
            _context = context;
            Parent = parent;
            PageDetails = new Pagination(FilterTabs);
            FilterTabs();
        }

        public ObservableCollection<TabDetails> Tabs { get; set; } = new();
        public void FilterTabs()
        {
            var search = SearchTab?.ToLowerInvariant() ?? string.Empty;
            var query = _context.Tabs
                .Where(c => c.IsClose == true &&
            (c.CustomerName.ToLower().Contains(search) ||
            c.Date.ToString().Contains(search) ||
            c.TabId.ToString().Contains(search)));
            UpdateTotalPages(query.Count());
            var tabs = query
                .OrderByDescending(c=>c.Date)
                .Select(c => new TabDetails(c))
                .Skip(PageDetails.ItemsPerPage * (PageDetails.CurrentPage))
                .Take(PageDetails.ItemsPerPage)
                .ToList();
            Tabs.Clear();
            foreach (var tab in tabs) Tabs.Add(tab);
        }
        public string _searchTab;
        public string SearchTab
        {
            get => _searchTab;
            set
            {
                _searchTab = value;
                FilterTabs();
            }
        }
        public TabDetails _selectedTab;
        public TabDetails SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged();
                LoadDetails();
            }
        }
        public ObservableCollection<OrderItemName> Orders { get; set; } = new();
        public ObservableCollection<PriceModifiedView> PriceModifiers { get; set; } = new();
        public string Name { get; set; }
        public string SubTotal { get; set; }
        public string Tip { get; set; }
        public string Total { get;set; }
        public void LoadDetails()
        {
            if (SelectedTab == null) return;
            var tab = _context.Tabs
                .Include(c=>c.OrderLists)
                .Include(c=>c.PriceModifiers)
                .First(c=>c.TabId == SelectedTab.TabId);
            Orders.Clear();
            Name = tab.CustomerName;
            SubTotal = $"₱{tab.OrderLists.Sum(c => c.Total):N2}";
            Tip = $"₱{tab.Tip:N2}";
            Total = $"₱{tab.Total:N2}";
            Orders.Clear();
            foreach (var item in tab.OrderLists)
            {
                var newOrderItem = new OrderItemName(item);
                Orders.Add(newOrderItem);
            }
            PriceModifiers.Clear();
            foreach (var item in tab.PriceModifiers)
            {
                var mod = new PriceModifiedView(item);
                PriceModifiers.Add(mod);
            }
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(Tip));
            OnPropertyChanged(nameof(Total));
        }
        private void UpdateTotalPages(int totalCount)
        {
            PageDetails.TotalPages = (int)Math.Ceiling((float)totalCount / PageDetails.ItemsPerPage);

            OnPropertyChanged(nameof(PageDetails));
        }
        public void Back()
        {
            DialogResult = true;
            Parent.CheckViewTabs();
        }
        public bool DialogResult { get; set; }
        public void Remove(TabDetails item)
        {
            var tab = _context.Tabs
                .Include(c=>c.OrderLists)
                .First(c=>c.TabId==item.TabId);
            tab.OrderLists.Clear();
            try
            {
                _context.Tabs.Remove(tab);
                _context.SaveChanges();
                FilterTabs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }        
        }
        public void OpenTab(TabDetails item)
        {
            var tab = _context.Tabs.First(c=>c.TabId== item.TabId);
            tab.IsClose = false;
            try
            {
                _context.SaveChanges();
                FilterTabs();
                Parent.LoadOpenTabs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
    }
}
