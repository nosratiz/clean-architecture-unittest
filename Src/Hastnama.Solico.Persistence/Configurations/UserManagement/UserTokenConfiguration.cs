using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.UserManagement
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.Property(e => e.Id).HasColumnName("UserTokenId")
                .IsRequired();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.UserAgent).IsRequired();

            builder.Property(e => e.Token).IsRequired();

            builder.Property(e => e.Ip).IsRequired();

            builder.Property(e => e.CreateDate).IsRequired();

            builder.Property(e => e.ExpireDate).IsRequired();

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Customer)
                .WithMany(e => e.UserTokens)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}