using System;
using System.Collections.Generic;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class OrderStatusHistory
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int OrderStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public List<HistoryOrderItem> OrderItems { get; set; }

        public virtual Order Order { get; set; }
    }

    public class HistoryOrderItem
    {
        public Guid Id { get; set; }

        public long ProductId { get; set; }
        
        public string ProductName { get; set; }

        public string ProductImage { get; set; }
        
        public int Count { get; set; }
        
        public long Price { get; set; }

        public DateTime CreateDate { get; set; }
    }
}