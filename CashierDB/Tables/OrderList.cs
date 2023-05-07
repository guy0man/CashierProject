using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Tables
{
    public class OrderList
    {
        public int OrderListId { get; set; }
        public int MenuItemId { get; set; }
        public int TabId { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
        public float RealTotal { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsOld { get; set; }
        public bool IsServed { get; set; }
        public int ServedQuantity { get; set; }
        //links
        public Tab TabLink { get; set; }
        public MenuItem MenuItemLink { get; set; }
    }
}
