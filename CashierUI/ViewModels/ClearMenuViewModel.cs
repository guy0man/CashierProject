using CashierDB;
using CashierDB.Tables;
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
    public class ClearMenuViewModel : ClearAllTabsViewModel,  INotifyPropertyChanged
    {            
        public ClearMenuViewModel(CashierContext context, MainTabViewModel parent) : base(context, parent)
        {
            _context = context;
            Parent = parent;
            WarningDescription = $"To confirm the permanent deletion of the current MENU type \"DELETE\" in the input box";
        }             
        public override void Confirm()
        {
            bool isValid = Validate();
            if (isValid)
            {
                var menuitems = _context.MenuItems.Include(c => c.OrderLists);
                var types = _context.ItemTypes;
                if (menuitems.Count() == 0) return;                
                try
                {
                    foreach (var type in types) _context.Remove(type);
                    foreach (var item in menuitems)
                    {
                        if (item.OrderLists != null) item.OrderLists.Clear();
                        _context.Remove(item);
                    }
                    _context.SaveChanges();
                    Parent.LoadMenuTabs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }
            }
        }
    }
}
