using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierUI.Dto
{
    public class PriceModifiersName
    {
        public int PriceModifiersID { get; set; }
        public string Name { get; set; }
        public string Percentage { get; set; }
        public PriceModifiersName(int id, string name, int percentage, bool isAdd)
        {
            PriceModifiersID = id;
            Name = name;
            if (isAdd) Percentage = $"+% {percentage}";
            else Percentage = $"-% {percentage}";
        }
    }
}
