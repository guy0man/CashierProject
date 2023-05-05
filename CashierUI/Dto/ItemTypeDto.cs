using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierUI.Dto
{
    public class ItemTypeName
    {
        public int ItemTypeId { get; set; }
        public string Name { get; set; }
        public ItemTypeName(int id, string name)
        {
            ItemTypeId = id;
            Name = name;
        }
    }
}
