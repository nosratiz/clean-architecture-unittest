using System.Collections.Generic;
using Hastnama.Solico.Domain.Models.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Hastnama.Solico.Persistence.Configurations.Shop
{
    public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("OrderHistoryConfigurationId");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreateDate).IsRequired();

            builder.HasOne(e => e.Order)
                .WithMany(e => e.OrderStatusHistories)
                .HasForeignKey(e => e.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
           
            
            builder.Property(e => e.OrderItems)
                .IsRequired()
                .HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include ,ReferenceLoopHandling = ReferenceLoopHandling.Ignore}),
                    v => JsonConvert.DeserializeObject<List<HistoryOrderItem>>(v,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include ,ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
        }
    }
}