using CashierDB.Configurations;
using CashierDB.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB
{
    public class CashierContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            if (!ob.IsConfigured) ob.UseSqlServer(@"Server=DESKTOP-A35B2UL\TACO;Initial Catalog=CashierDB;Trusted_Connection=True");
        }       
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderList> OrderLists { get; set; }
        public DbSet<Tab> Tabs { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<PriceModifier> PriceModifiers { get; set; }
        public DbSet<PriceModifiersApplied> PriceModifiersApplications { get; set; }
        public DbSet<Company> Company { get; set; }
        protected override void OnModelCreating(ModelBuilder mb)
        {          
            mb.ApplyConfiguration(new MenuItemConfiguration());
            mb.ApplyConfiguration(new OrderListConfiguration());
            mb.ApplyConfiguration(new TabConfiguration());
            mb.ApplyConfiguration(new ItemTypeConfiguration()); 
            mb.ApplyConfiguration(new  PriceModifierConfiguration());
            mb.ApplyConfiguration(new PriceModifiersAppliedConfiguration());
            mb.ApplyConfiguration(new CompanyConfiguration());
        }
    }    
}
