using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.UserManagement
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.Property(e => e.Id).HasColumnName("UserAddressId")
                .IsRequired().UseIdentityColumn();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.Address).IsRequired();

            builder.Property(e => e.Mobile).IsRequired();

            builder.HasQueryFilter(x => x.IsDeleted == false);

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserAddresses)
                .HasForeignKey(e => e.UserId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}