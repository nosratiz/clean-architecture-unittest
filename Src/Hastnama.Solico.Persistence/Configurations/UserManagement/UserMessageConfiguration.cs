using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.UserManagement
{
    public class UserMessageConfiguration : IEntityTypeConfiguration<UserMessage>
    {
        public void Configure(EntityTypeBuilder<UserMessage> builder)
        {
            builder.Property(e => e.Id).HasColumnName("UserMessageId").IsRequired();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.SendDate).IsRequired();

            builder.Property(e => e.CustomerHasDeleted).IsRequired();

            builder.Property(e => e.UserHasDeleted).IsRequired();

            builder.HasOne(e => e.Customer)
                .WithMany(e => e.UserMessages)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.User)
                .WithMany(e => e.UserMessages)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Message)
                .WithMany(e => e.UserMessages)
                .HasForeignKey(e => e.MessageId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}