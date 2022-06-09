using System;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class ProductMedia
    {
        public long Id { get; set; }

        public long ProductId { get; set; }

        public string Icon { get; set; }

        public string Description { get; set; }
        
        public virtual Product Product { get; set; }
    }
}