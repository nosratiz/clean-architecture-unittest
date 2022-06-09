using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.UserManagement
{
    public class CustomerEnrollmentConfiguration : IEntityTypeConfiguration<CustomerEnrollment>
    {
        public void Configure(EntityTypeBuilder<CustomerEnrollment> builder)
        {
            builder.Property(e => e.Id).IsRequired().UseIdentityColumn();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.SolicoCustomerId);

            builder.Property(e => e.IsDone).IsRequired();

            builder.Property(e => e.CreateDate).IsRequired();
        }
    }
}