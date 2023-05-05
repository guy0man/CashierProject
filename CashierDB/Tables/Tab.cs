using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Tables
{
    public class Tab
    {
        public int TabId { get; set; }  
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public float Total { get; set; }
        public float Tip { get; set; }
        public bool IsClose { get; set; }
        public bool IsPaid { get; set; }
        //links
        public ICollection<OrderList> OrderLists { get; set; }
    }
}
