﻿using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            #region 
            //builder.OwnsOne(o => o.ShipToAddress, a =>
            //{
            //    a.WithOwner();
            //    a.Property("FirstName").IsRequired();
            //    a.Property("LastName").IsRequired();
            //    a.Property("Street").IsRequired();
            //    a.Property("City").IsRequired();
            //    a.Property("State").IsRequired();
            //    a.Property("Zipcode").IsRequired();

            //}); 

            //builder.OwnsOne(o => o.ShipToAddress, a =>
            //  {
            //      a.WithOwner();
            //      a.Property(x => x.FirstName).HasColumnName("FirstName");
            //      a.Property(x => x.State).HasColumnName("State");
            //      a.Property(x => x.LastName).HasColumnName("LastName");
            //      a.Property(x => x.City).HasColumnName("City");
            //      a.Property(x => x.Street).HasColumnName("Street");
            //      a.Property(x => x.Zipcode).HasColumnName("Zipcode");

            //  });

            #endregion

            //builder.OwnsOne(o => o.ShipToAddress);


            builder.Property(s => s.Status)
                .HasConversion(
                    o => o.ToString(),
                    o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
                );

            //if we delete an order we also delete any order items
            //are part of this order
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
