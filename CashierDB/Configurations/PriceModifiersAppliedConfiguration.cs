using CashierDB.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Configurations
{
    public class PriceModifiersAppliedConfiguration : IEntityTypeConfiguration<PriceModifiersApplied>
    {
        public void Configure(EntityTypeBuilder<PriceModifiersApplied> d)
        {
            d.ToTable("PriceModifiersApplied");
        }
    }
}
