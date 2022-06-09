using System;
using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.UserManagement
{
    public class UserFileConfiguration : IEntityTypeConfiguration<UserFile>
    {
        public void Configure(EntityTypeBuilder<UserFile> builder)
        {
            builder.Property(e => e.Id).HasColumnName("UserFileId")
                .IsRequired().UseIdentityColumn();

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.UniqueId).IsRequired();

            builder.Property(e => e.Url).IsRequired();

            builder.Property(e => e.CreateDate).HasDefaultValue(DateTime.Now);
        }
    }
}