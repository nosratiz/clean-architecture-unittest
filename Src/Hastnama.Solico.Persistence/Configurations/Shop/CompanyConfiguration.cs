using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("CompanyId").UseIdentityColumn();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.Slug).IsRequired();

            builder.Property(e => e.CreateDate).IsRequired();

            builder.Property(e => e.IsDeleted).IsRequired();

            builder.HasQueryFilter(e => e.IsDeleted == false);

            
        }
    }
}