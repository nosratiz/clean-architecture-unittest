using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.UserManagement
{
    public class CustomerOpenItemConfiguration : IEntityTypeConfiguration<CustomerOpenItem>
    {
        public void Configure(EntityTypeBuilder<CustomerOpenItem> builder)
        {
            builder.Property(e => e.Id).HasComment("CustomerOpenItemId").IsRequired();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.DocumentNumber).IsRequired();

            builder.Property(e => e.DueDate).IsRequired();

            builder.Property(e => e.Amount).IsRequired();

            builder.Property(e => e.IsPaid).IsRequired();
        }
    }
}