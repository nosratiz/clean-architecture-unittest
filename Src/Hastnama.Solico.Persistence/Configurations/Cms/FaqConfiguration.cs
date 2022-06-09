using Hastnama.Solico.Domain.Models.Cms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Cms
{
    public class FaqConfiguration : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.Property(e => e.Id).HasColumnName("FaqId")
                .IsRequired().UseIdentityColumn();

            builder.Property(e => e.Answer).IsRequired();

            builder.Property(e => e.Question).IsRequired();
        }
    }
}