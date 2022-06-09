using System;
using System.Collections.Generic;

namespace Hastnama.Solico.Application.Cms.Dashboard.Dto
{
    public class DashboardDto
    {
        public int CustomerCount { get; set; }

        public int ProductCount { get; set; }

        public int OrderCount { get; set; }

        public List<LatestOrderDto> LatestOrders { get; set; }
        
        public List<WeeklySailedOrder> WeeklySailedOrders { get; set; }
        
    }

    public class LatestOrderDto
    {
        public string CustomerName { get; set; }

        public double FinalPrice { get; set; }

        public bool IsSuccess { get; set; }

        public int OrderStatus { get; set; }
    }

    public class WeeklySailedOrder
    {
        public DateTime Date { get; set; }

        public int OrderCount { get; set; }

        public double Amount { get; set; }
    }

 
}