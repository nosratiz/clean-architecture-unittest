using System;
using Hastnama.Solico.Domain.Models.Shop;
using Hastnama.Solico.Domain.Models.UserManagement;

namespace Hastnama.Solico.Domain.Models.Statistic
{
    public class CustomerProductView
    {
        public Guid CustomerId { get; set; }

        public long ProductId { get; set; }

        public int ViewCount { get; set; }
        
        public virtual Customer Customer { get; set; }
        
        public virtual Product Product { get; set; }
    }
}