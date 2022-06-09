using System;
using System.Collections.Generic;
using Hastnama.Solico.Application.Shop.Cart.Dto;
using Hastnama.Solico.Domain.Models.Shop;

namespace Hastnama.Solico.Application.Shop.Orders.Dto
{
    public class OrderStatusHistoryDto
    {
        public int OrderStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}