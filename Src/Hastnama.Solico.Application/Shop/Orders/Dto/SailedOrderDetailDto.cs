using System;
using System.Collections.Generic;

namespace Hastnama.Solico.Application.Shop.Orders.Dto
{
    public class SailedOrderDetailDto
    {
        public string MaterialId { get; set; }

        public long LineNumber { get; set; }

        public int Count { get; set; }

        public string Unit { get; set; }

        public double UnitPrice { get; set; }

        public double FinalPrice { get; set; }

        public double Discount { get; set; }

        public double Tax { get; set; }

        public double Levy { get; set; }
    }

    public class SailedOrderDto
    {
        public string QuotationNumber { get; set; }

        public string OrderNumber { get; set; }

        public DateTime CreateDate { get; set; }

        public string SolicoCustomerId { get; set; }

        public List<SailedOrderDetailDto> OrderDetail { get; set; }
    }
}