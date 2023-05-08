using CashierDB.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierUI.Dto
{
    public class CompanyDetails
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public CompanyDetails(Company company)
        {
            Name = company.Name;
            Address = company.Address;          
        }
    }
}
