using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class CustomerProductPriceConfiguration : IEntityTypeConfiguration<CustomerProductPrice>
    {
        public void Configure(EntityTypeBuilder<CustomerProductPrice> builder)
        {
            builder.HasKey(e => new { e.CustomerId, e.ProductId });
            
            builder.HasOne(e => e.Customer)
                .WithMany(e => e.CustomerProductPrices)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(e => e.Product)
                .WithMany(e => e.CustomerProductPrices)
                .HasForeignKey(e => e.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Price).IsRequired();

            builder.Property(e => e.SyncDate).IsRequired();
        }
    }
}