using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Tables
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string? Name { get; set;}
        public string? Address { get; set; }
        //links
        public ICollection<Tab> Tabs { get; set; }
    }
}
