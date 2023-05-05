﻿using CashierDB;
using CashierDB.Tables;
using CashierUI.Dto;
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
                var tab = _context.Tabs.First(c=>c.TabId == TabToEdit.TabId);
                tab.CustomerName = Name;
                tab.Total = float.Parse(TabTotal.Remove(0, 1));
                tab.OrderLists = new List<OrderList>();
                foreach (var order in Orders)
                {
                    tab.OrderLists.Add(new OrderList()
                    {
                        MenuItemId = order.MenuItemId,
                        Quantity = order.Quantity,
                        Total = order.Total,
                        RealTotal = order.RealTotal,
                        IsCanceled = order.IsCanceled
                    });
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
            Parent.CheckEditTab();
        }
    }
}
