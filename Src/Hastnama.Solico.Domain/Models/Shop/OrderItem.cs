using System;
using Hastnama.Solico.Domain.Models.UserManagement;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public long ProductId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Order Order { get; set; }
    }
}