using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.UserManagement
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("CustomerId");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.SolicoCustomerId).IsRequired();

            builder.Property(e => e.SyncDate).IsRequired();

            builder.Property(e => e.IsDeleted).IsRequired();

            builder.HasQueryFilter(e => e.IsDeleted == false);

            builder.Property(e => e.IsShowInLastSync).IsRequired();
        }
    }
}