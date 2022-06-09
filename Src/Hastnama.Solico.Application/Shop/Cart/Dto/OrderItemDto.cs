using System;

namespace Hastnama.Solico.Application.Shop.Cart.Dto
{
    public class OrderItemDto
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