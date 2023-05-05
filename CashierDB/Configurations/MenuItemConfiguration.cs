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
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> d)
        {
            d.ToTable("MenuItem");          
            d.HasData(new MenuItem {MenuItemId = 1, Name = "Fried Chicken w/ Rice", ItemTypeId = 1, Price = 150, Stock = 25});
            d.HasData(new MenuItem {MenuItemId = 2, Name = "Ribs w/ Rice", ItemTypeId = 1, Price = 175, Stock = 25 });
            d.HasData(new MenuItem {MenuItemId = 3, Name = "Sisig w/ Rice", ItemTypeId = 1, Price = 125, Stock = 25 });
            d.HasData(new MenuItem {MenuItemId = 4, Name = "BTS Drink", ItemTypeId = 2, Price = 95, Stock = 50 });
            d.HasData(new MenuItem {MenuItemId = 5, Name = "Sola", ItemTypeId = 2, Price = 99, Stock = 50 });
            d.HasData(new MenuItem {MenuItemId = 6, Name = "Soft Drink in Can", ItemTypeId = 2, Price = 75, Stock = 50 });
            d.HasData(new MenuItem {MenuItemId = 7, Name = "Cheesecake", ItemTypeId = 3, Price = 200, Stock = 20 });
            d.HasData(new MenuItem {MenuItemId = 8, Name = "Moist Cake", ItemTypeId = 3, Price = 150, Stock = 20 });
            d.HasData(new MenuItem {MenuItemId = 9, Name = "Sundae", ItemTypeId = 3, Price = 100, Stock = 20 });
        }
    }
}
