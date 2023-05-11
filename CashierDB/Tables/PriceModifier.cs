using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Tables
{
    public class PriceModifier
    {
        public int PriceModifierId {get; set;}
        public string Name { get; set;}
        public double Percentage { get; set;}
        public bool IsAdd { get; set;}
        public bool AutoApply { get; set;}
        //links
        public ICollection<PriceModifiersApplied> Tabs { get; set;}           
    }
}
