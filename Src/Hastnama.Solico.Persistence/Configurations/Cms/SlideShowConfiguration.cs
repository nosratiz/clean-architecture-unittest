using Hastnama.Solico.Domain.Models.Cms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Cms
{
    public class SlideShowConfiguration: IEntityTypeConfiguration<SlideShow>
    {
        public void Configure(EntityTypeBuilder<SlideShow> builder)
        {
            builder.Property(e => e.Id).HasColumnName("SliderId").IsRequired().UseIdentityColumn();

            builder.HasKey(e => e.Id);


            builder.Property(e => e.Image).IsRequired();

            builder.Property(e => e.Name).IsRequired();
        }
    }
}