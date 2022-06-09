using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class ProductGalleryConfiguration : IEntityTypeConfiguration<ProductGallery>
    {
        public void Configure(EntityTypeBuilder<ProductGallery> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ProductGalleryId").IsRequired().UseIdentityColumn();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Image).IsRequired();

            builder
                .HasOne(e => e.Product)
                .WithMany(e => e.ProductGalleries)
                .HasForeignKey(e => e.ProductId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}