using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Cms
{
    public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
    {
        public void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            builder.Property(e => e.Id).HasColumnName("AppSettingId")
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(e => e.Title).IsRequired();

            builder.Property(e => e.MaxSizeUploadFile).IsRequired().HasDefaultValue(3);

            builder
                .HasOne(e => e.User)
                .WithOne(e => e.AppSetting)
                .IsRequired().HasForeignKey<AppSetting>(e => e.UserId);
        }
    }
}