using System;
using System.Collections.Generic;
using Hastnama.Solico.Application.Shop.Cart.Dto;
using Hastnama.Solico.Domain.Models.Shop;

namespace Hastnama.Solico.Application.Shop.Orders.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int OrderStatus { get; set; }
        public long FinalAmount { get; set; }
        public long DiscountPrice { get; set; }
        public long ShipmentPrice { get; set; }
        public long Tax { get; set; }

        public string Description { get; set; }
        public string OrderNumber { get; set; }
        public string Address { get; set; }
        public string DeliveryName { get; set; }
        public string QuotationNumber { get; set; }
        public bool IsPaid { get; set; }


        public bool IsSuccess { get; set; }

        public DateTime CreateDate { get; set; }
        
        public List<OrderItemDto> OrderItems { get; set; }
        
        public List<OrderStatusHistoryDto> OrderStatusHistories { get; set; }

    }
}