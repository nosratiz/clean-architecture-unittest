using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Dashboard.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Cms.Dashboard.Queries
{
    public class GetDashboardQuery : IRequest<DashboardDto>
    {
    }


    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
    {
        private readonly ISolicoDbContext _context;

        public GetDashboardQueryHandler(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var WeeklySailedOrders = await GetWeeklySailedOrders(cancellationToken);
            
            var weeklySailed = CalculateWeeklySailedOrders(WeeklySailedOrders);
            
            return new DashboardDto
            {
                ProductCount =  await _context.Products.CountAsync(cancellationToken),
                CustomerCount =  await _context.Customers.CountAsync(cancellationToken),
                OrderCount =   await _context.Orders.CountAsync(cancellationToken),
                LatestOrders = await GetLatestOrder(cancellationToken),
                WeeklySailedOrders = weeklySailed
            };
        }

        #region Query

        private static List<WeeklySailedOrder> CalculateWeeklySailedOrders(List<WeeklySailedOrder> WeeklySailedOrders)
        {
            List<WeeklySailedOrder> weeklySailed = new List<WeeklySailedOrder>();
            for (int i = 0; i <= 6; i++)
            {
                var date = DateTime.Now.AddDays(-i).Date;
                var existDate = WeeklySailedOrders
                    .FirstOrDefault(x => x.Date == date);

                if (existDate is null)
                {
                    weeklySailed.Add(new WeeklySailedOrder
                    {
                        Date = date,
                        OrderCount = 0,
                        Amount = 0,
                        
                    });
                }
                else
                {
                    weeklySailed.Add(new WeeklySailedOrder
                    {
                        Date = date,
                        OrderCount = existDate.OrderCount,
                        Amount = existDate.Amount
                    });
                }
            }

            return weeklySailed;
        }

        private async Task<List<WeeklySailedOrder>> GetWeeklySailedOrders(CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(x => x.CreateDate.Date >= DateTime.Now.Date.AddDays(-7) && x.CreateDate.Date <= DateTime.Now)
                .GroupBy(x => x.CreateDate.Date).Select(x => new WeeklySailedOrder
                {
                    Date = x.Key,
                    OrderCount = x.Count(),
                    Amount = x.Sum(e=>e.FinalAmount)
                }).ToListAsync(cancellationToken);
        }

        private async Task<List<LatestOrderDto>> GetLatestOrder(CancellationToken cancellationToken)
        {
            return await _context.Orders
                .OrderByDescending(x => x.CreateDate).Take(5).Select(x => new LatestOrderDto
                {
                    CustomerName = x.DeliveryName,
                    FinalPrice = x.FinalAmount,
                    IsSuccess = x.IsSuccess,
                    OrderStatus = x.OrderStatus
                })
                .ToListAsync(cancellationToken);
        }

        #endregion
    
    }
}