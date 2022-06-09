using Hastnama.Solico.Domain.Models.Cms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Cms
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(e => e.Id).HasColumnName("MessageId").IsRequired();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Content).IsRequired();

            builder.Property(e => e.CreateDate).IsRequired();

            builder.Property(e => e.Title).IsRequired();
        }
    }
}