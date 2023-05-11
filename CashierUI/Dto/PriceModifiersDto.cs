using CashierDB.Tables;
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
        public string AutoApply { get; set; }
        public bool boolAutoApply { get; set; }
        public bool Applied { get; set; }
        public PriceModifiersName(int id, string name, double percentage, bool isAdd, bool autoApply)
        {
            PriceModifiersID = id;
            Name = name;
            boolAutoApply = autoApply;
            if (isAdd) Percentage = $"+{percentage * 100}%";
            else Percentage = $"-{percentage * 100}%";
            if (autoApply)
            {
                AutoApply = $"Auto-Apply";
                Applied = true ;
            }
            else
            {
                AutoApply = string.Empty;
                Applied = false;
            }

        }
    }
    public class PriceModifiedView
    {
        public int PriceModifierId { get; set; }
        public string Name { get; set; }
        public string Total { get; set; }
        public bool IsAdd { get; set; }
        public PriceModifiedView(PriceModifiersApplied pm)
        {
            PriceModifierId = pm.PriceModifierLink.PriceModifierId;
            string sign;
            if (pm.PriceModifierLink.IsAdd) sign = "+";
            else sign = "-";
            Name = $"{pm.PriceModifierLink.Name} ({sign}{pm.PriceModifierLink.Percentage * 100}%)";          
            Total = $"₱{pm.Total:N2}";
            IsAdd = pm.PriceModifierLink.IsAdd;
        }
    }
}
