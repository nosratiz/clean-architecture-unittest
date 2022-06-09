using System;
using System.Collections.Generic;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            OrderStatusHistories = new HashSet<OrderStatusHistory>();
        }

        public Guid Id { get; set; }

        public long OrderIndex{ get; set; }
        public int OrderStatus { get; set; }
        public double FinalAmount { get; set; }
        public double DiscountPrice { get; set; }
        
        public double Tax { get; set; }
        public long ShipmentPrice { get; set; }

        public string Description { get; set; }
        public string OrderNumber { get; set; }
        public string Address { get; set; }
        public string DeliveryName { get; set; }
        public string QuotationNumber { get; set; }

        public string SolicoCustomerId { get; set; }


        public bool IsSuccess { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsPaid { get; set; }

        public bool IsRegister { get; set; }

        public DateTime CreateDate { get; set; }



        public virtual ICollection<OrderItem> OrderItems { get; }
        public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; }
        public virtual ICollection<BankTransaction> BankTransactions { get; set; }

    }
}