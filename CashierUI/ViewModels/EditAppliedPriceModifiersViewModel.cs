using CashierDB;
using CashierDB.Tables;
using CashierUI.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierUI.ViewModels
{
    public class EditAppliedPriceModifiersViewModel
    {
        public CashierContext _context { get; set; }
        public PayWindowViewModel Parent { get; set; }
        public OpenTabDetails Tab { get; set; }
        public EditAppliedPriceModifiersViewModel(CashierContext context, PayWindowViewModel parent)
        {
            _context = context;
            Parent = parent;
            Tab = Parent.Tab;
            PriceModifiers= Parent.Parent.PriceModifiers;
        }
        public ObservableCollection<PriceModifiersName> PriceModifiers { get; set; } = new();
        public void Save()
        {
            var tab = _context.Tabs.First(c=>c.TabId == Tab.TabId);
            tab.PriceModifiers = new List<PriceModifiersApplied>();
            foreach(var pm in PriceModifiers.Where(c=>c.Applied == true))
            {
                var mod = _context.PriceModifiers.First(c => c.PriceModifierId == pm.PriceModifiersID);
                var application = new PriceModifiersApplied();
                application.PriceModifierId = pm.PriceModifiersID;
                application.Total = tab.Total * (float)(mod.Percentage);
                tab.PriceModifiers.Add(application);
            }
            try
            {
                _context.SaveChanges();
                DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
                DialogResult = false;
            }
            Parent.CheckEditPM();
        }
        public void Cancel()
        {
            DialogResult = true;
            Parent.CheckEditPM();
        }
        public bool DialogResult { get; set; }
    }
}
