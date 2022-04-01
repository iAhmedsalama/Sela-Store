using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; } 

        public DbSet<ProductType> ProductTypes { get; set; }

        //adding order Aggregation DBSets
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Product>().Property(p => p.Description).IsRequired().HasMaxLength(500);

            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>().Property(p => p.PictureUrl).IsRequired();

            modelBuilder.Entity<Product>().HasOne(b => b.ProductBrand).WithMany()
                .HasForeignKey(b => b.ProductBrandId);

            modelBuilder.Entity<Product>().HasOne(t => t.ProductType).WithMany()
                .HasForeignKey(p => p.ProductTypeId);

            //adding configration for order aggregate

            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.ShipToAddress);

            modelBuilder.Entity<OrderItem>()
                .OwnsOne(oi => oi.ItemOrdered);


            base.OnModelCreating(modelBuilder);

           

        }


    }
}
