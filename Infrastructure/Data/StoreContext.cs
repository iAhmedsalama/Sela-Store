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
            modelBuilder.Entity<Address>().HasNoKey();
            modelBuilder.Entity<ProductItemOrdered>().HasNoKey();



            base.OnModelCreating(modelBuilder);



            #region coversion from decimal to double in other database types
            /*
            if (Database.ProviderName == "Microsoft.EntityFramworkCore.SqlServer")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));


                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                            .HasConversion<double>();
                    }
                }
            }
            */
            #endregion

        }


        #region add new configration using model builder in another file
        //add new configration using model builder in another file 
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        */
        #endregion


    }
}
