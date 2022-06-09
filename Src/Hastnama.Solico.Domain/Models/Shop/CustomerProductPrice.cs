using System;
using Hastnama.Solico.Domain.Models.UserManagement;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class CustomerProductPrice
    {
        public long ProductId { get; set; }

        public Guid CustomerId { get; set; }

        public double Price { get; set; }

        public DateTime SyncDate { get; set; }
        
        public virtual Customer Customer { get; set; }
        
        public virtual Product Product { get; set; }
    }
}