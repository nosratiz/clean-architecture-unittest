using Hastnama.Solico.Domain.Models.Statistic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class CustomerProductViewConfiguration : IEntityTypeConfiguration<CustomerProductView>
    {
        public void Configure(EntityTypeBuilder<CustomerProductView> builder)
        {
            builder.HasKey(e => new { e.CustomerId, e.ProductId });

            builder.HasOne(e => e.Customer)
                .WithMany(e => e.CustomerProductViews)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(e => e.Product)
                .WithMany(e => e.CustomerProductViews)
                .HasForeignKey(e => e.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}