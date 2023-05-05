using CashierDB;
using CashierUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierUI.ViewModels
{
    public class EditMenuItemViewModel : AddMenuItemViewModel
    {
        public CashierContext _context { get; set; }    
        public MenuTabViewModel Parent { get; set; }       
        public MenuItemName ItemToEdit { get; set; }
        public EditMenuItemViewModel(CashierContext context, MenuItemName itemToEdit, MenuTabViewModel parent) : base (context, parent)
        {
            _context = context;
            ItemToEdit = itemToEdit;
            Parent = parent;
            Name = itemToEdit.Name;     
            _selectedType = Types.First(c=>c.ItemTypeId == itemToEdit.ItemTypeId);
            Price = itemToEdit.Price.Remove(0,1);
            Stock = itemToEdit.Stock.ToString();
        }
        public override void Add()
        {
            bool isValid = Validate();
            if (isValid )
            {
                var item = _context.MenuItems.First(c => c.MenuItemId == ItemToEdit.MenuItemId);
                item.Name = Name;
                item.ItemTypeId = SelectedType.ItemTypeId;
                item.Price = float.Parse(Price);
                item.Stock = int.Parse(Stock);               
                try
                {
                    _context.SaveChanges();
                    DialogResult = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    DialogResult = false;
                }

            }
            Parent.EditMenuCheck();
        }
        public override void Cancel()
        {
            DialogResult = true;
            Parent.EditMenuCheck();
        }
    }
}
