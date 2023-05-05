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
    public class ItemTypeConfiguration : IEntityTypeConfiguration<ItemType>
    {
        public void Configure(EntityTypeBuilder<ItemType> d)
        {
            d.ToTable("ItemType");
            d.HasData(new ItemType { ItemTypeId = 1, Name = "Entree"});
            d.HasData(new ItemType { ItemTypeId = 2, Name = "Drink"});
            d.HasData(new ItemType { ItemTypeId = 3, Name = "Dessert"});
        }
    }
}
