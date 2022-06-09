using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("OrderId");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.OrderIndex).IsRequired().UseIdentityColumn();

            builder.Property(e => e.CreateDate).IsRequired();

            builder.Property(e => e.DeliveryName).IsRequired();

            builder.Property(e => e.Address).IsRequired();

            builder.Property(e => e.FinalAmount).IsRequired();

            builder.Property(e => e.DiscountPrice).IsRequired();

            builder.Property(e => e.IsDeleted).IsRequired();

            builder.Property(e => e.OrderStatus).IsRequired();

            builder.Property(e => e.OrderNumber).IsRequired();

            builder.Property(e => e.ShipmentPrice).IsRequired();

            builder.Property(e => e.IsPaid).IsRequired();

            builder.Property(e => e.SolicoCustomerId).IsRequired();

            builder.HasQueryFilter(e => e.IsDeleted == false);
        }
    }
}