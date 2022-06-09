using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.UserManagement
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("UserId")
                .IsRequired()
                .UseIdentityColumn();

            builder.HasKey(e => e.Id);

            builder.HasQueryFilter(x => x.IsDeleted == false);


            builder.Property(e => e.ActivationCode).IsRequired();

            builder.Property(e => e.IsEmailConfirmed).HasDefaultValue(false);

            builder.Property(e => e.IsPhoneConfirmed).HasDefaultValue(false);

            builder.Property(e => e.RegisterDate).IsRequired();

            builder.Property(e => e.ExpiredVerification).IsRequired();

            builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);

            builder.HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId);
        }
    }
}