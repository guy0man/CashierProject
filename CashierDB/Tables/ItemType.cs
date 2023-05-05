using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Tables
{
    public class ItemType
    {
        public int ItemTypeId { get; set; } 
        public string Name { get; set; }
        //links
        public ICollection<MenuItem> MenuItems { get; set; }
    }
}
