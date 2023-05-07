using CashierDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierUI.ViewModels
{
    public class ClearDatabaseViewModel : ClearAllTabsViewModel, INotifyPropertyChanged
    {
        public ClearDatabaseViewModel(CashierContext context, MainTabViewModel parent) : base(context, parent)
        {
            _context = context;
            Parent = parent;
            WarningDescription = $"To confirm the permanent deletion of all the CONTENTS OF THE DATABASE type \"DELETE\" in the input box";
        }
        public override void Confirm()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var tabs = _context.Tabs.Include(c=>c.OrderLists);
                var menuitems = _context.MenuItems.Include(c => c.OrderLists);
                var types = _context.ItemTypes;
                if (tabs.Count() == 0 && menuitems.Count() == 0 && types.Count() == 0) return;
                try
                {
                    foreach (var tab in tabs)
                    {
                        if (tab.OrderLists != null) tab.OrderLists.Clear();
                        _context.Remove(tab);
                    }
                    foreach (var item in menuitems)
                    {
                        if (item.OrderLists != null) item.OrderLists.Clear();
                        _context.Remove(item);
                    }
                    foreach (var type in types) _context.Remove(type);
                    _context.SaveChanges();
                    Parent.LoadMenuTabs();
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
