using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Tables
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }        
        public int ItemTypeId { get; set; }
        public string Name { get; set; }  
        public float Price { get; set; }
        public int Stock { get; set; }
        //links
        public ICollection<OrderList> OrderLists { get; set; }    
        public ItemType ItemTypeLink { get; set; }
    }
}
