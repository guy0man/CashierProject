using CashierDB;
using CashierDB.Tables;
using CashierUI.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CashierUI.ViewModels
{
    public class EditTabViewModel : AddTabViewModel, INotifyPropertyChanged
    {       
        public CashierContext _context { get; set; }
        public MainTabViewModel Parent { get; set; }
        public OpenTabDetails TabToEdit { get; set; }
        public EditTabViewModel(CashierContext context, MainTabViewModel parent, OpenTabDetails tabToEdit) : base(context, parent)
        {
            _context = context;
            Parent = parent;
            TabToEdit = tabToEdit;
            Name = tabToEdit.Name;
            var order = new List<PartialOrderItem>();
            foreach (var item in tabToEdit.Orders)
            {
                var MenuItem = _context.MenuItems.First(c => c.MenuItemId == item.MenuItemId);
                var menuItem = new MenuItemName(MenuItem);
                var orderItem = new PartialOrderItem(menuItem);
                orderItem.Quantity = item.Quantity;
                CalculateTotal(orderItem.Quantity,orderItem);
                orderItem.IsCanceled = item.IsCanceled;
                orderItem.IsServed = item.IsServed;
                orderItem.ServedQuantity = item.ServedQuantity;
                orderItem.IsOld = true;
                Orders.Add(orderItem);
            }
            LoadTotal();
            LoadMenuTabs();
        }
        public override void Add()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var tab = _context.Tabs.Include(c=>c.PriceModifiers).ThenInclude(c=>c.PriceModifierLink).First(c => c.TabId == TabToEdit.TabId);
                tab.CustomerName = Name;
                tab.Total = float.Parse(TabTotal.Remove(0, 1));
                tab.OrderLists = new List<OrderList>();
                foreach (var order in Orders)
                {
                    var item = new OrderList();
                    item.MenuItemId = order.MenuItemId;
                    item.Quantity = order.Quantity;
                    item.Total = order.Total;
                    item.RealTotal = order.RealTotal;
                    item.IsCanceled = order.IsCanceled;
                    if (order.ServedQuantity != order.Quantity) item.IsServed = false;
                    else item.IsServed = order.IsServed;
                    item.ServedQuantity = order.ServedQuantity;
                    tab.OrderLists.Add(item);                
                }
                foreach(var item in tab.PriceModifiers)
                {
                    item.Total = tab.Total * (float)(item.PriceModifierLink.Percentage/100);
                }
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
            Parent.CheckEditTab();
        }
        public override void Cancel()
        {
            DialogResult = true;
            Parent.CheckCancelEditTab();
        }
    }
}
