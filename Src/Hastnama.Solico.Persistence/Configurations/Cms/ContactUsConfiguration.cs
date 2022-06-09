using System;
using Hastnama.Solico.Domain.Models.Cms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Cms
{
    public class ContactUsConfiguration : IEntityTypeConfiguration<ContactUs>
    {
        public void Configure(EntityTypeBuilder<ContactUs> builder)
        {
            builder.Property(e => e.Id).HasColumnName("ContactUsId").IsRequired().UseIdentityColumn();

            builder.Property(e => e.CreateDate).HasDefaultValue(DateTime.Now);

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.Email).IsRequired();

            builder.Property(e => e.Message).IsRequired();
        }
    }
}