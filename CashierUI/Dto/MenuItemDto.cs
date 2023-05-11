using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashierDB.Tables;

namespace CashierUI.Dto
{
    public class MenuItemName
    {
        public int MenuItemId { get; set; }
        public int ItemTypeId { get; set; }
        public string Name { get; set; } 
        public string Type { get; set; }
        public string Price { get; set; }
        public int Stock { get; set; }
        public MenuItemName (MenuItem item)
        {
            MenuItemId = item.MenuItemId;
            ItemTypeId = item.ItemTypeId;
            Name = item.Name;
            Type = item.ItemTypeLink.Name;
            Price = $"₱{item.Price:N2}";
            Stock = item.Stock;
        }
    }
}
