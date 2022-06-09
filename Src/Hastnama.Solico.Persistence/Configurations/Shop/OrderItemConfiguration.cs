using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(e => e.Id).HasColumnName("OrderItemId").IsRequired();

            builder.Property(e => e.Count).IsRequired();

            builder.Property(e => e.Price).IsRequired();

            builder.Property(e => e.CreateDate).IsRequired();

            builder.HasOne(e => e.Order)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Customer)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.CustomerId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Product)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.ProductId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}