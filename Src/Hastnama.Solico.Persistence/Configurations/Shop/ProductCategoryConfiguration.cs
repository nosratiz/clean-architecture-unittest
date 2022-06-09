using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("ProductCategoryId").UseIdentityColumn();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired();


            builder.HasQueryFilter(e => e.IsDeleted == false);

            builder.HasMany(e => e.Children)
                .WithOne(e => e.Parent)
                .HasForeignKey(e => e.ParentId);

            builder.HasMany(e => e.Products)
                .WithOne(e => e.ProductCategory)
                .HasForeignKey(e => e.ProductCategoryId);
        }
    }
}