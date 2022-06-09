using Hastnama.Solico.Domain.Models.Cms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Cms
{
    public class CustomerConsultConfiguration : IEntityTypeConfiguration<CustomerConsult>
    {
        public void Configure(EntityTypeBuilder<CustomerConsult> builder)
        {
            builder.Property(e => e.Id).HasColumnName("CustomerConsultId").IsRequired();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreateDate).IsRequired();

            builder.Property(e => e.IsDelete).IsRequired();

            builder.Property(e => e.IsSettle).IsRequired();

            builder.HasQueryFilter(e => e.IsDelete == false);

            builder
                .HasOne(e => e.Customer)
                .WithMany(e => e.CustomerConsults)
                .HasForeignKey(e => e.CustomerId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}