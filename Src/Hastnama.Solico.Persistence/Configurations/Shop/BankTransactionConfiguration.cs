using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
    {
        public void Configure(EntityTypeBuilder<BankTransaction> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("BankTransactionId");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Price).IsRequired();

            builder.Property(e => e.Token).IsRequired();

            builder.Property(e => e.CreateDate).IsRequired();

            builder.Property(e => e.Status).IsRequired();

            builder.HasOne(e => e.Order)
                .WithMany(e => e.BankTransactions)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(e => e.CustomerOpenItem)
                .WithMany(e => e.BankTransactions)
                .HasForeignKey(e => e.CustomerOpenItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}