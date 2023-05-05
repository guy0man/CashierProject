using CashierDB.Tables;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashierDB.Configurations
{
    public class TabConfiguration : IEntityTypeConfiguration<Tab> 
    {
        public void Configure(EntityTypeBuilder<Tab> d)
        {
            d.ToTable("Tab");
        }
    }
}
