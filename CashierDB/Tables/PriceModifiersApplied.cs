using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Tables
{
    public class PriceModifiersApplied
    {
        public int PriceModifiersAppliedId { get; set; }
        public int TabId { get; set; }
        public int PriceModifier { get; set; }
        //links
        public Tab TabLink { get; set; }
        public PriceModifier PriceModifierLink { get; set; }
    }
}
