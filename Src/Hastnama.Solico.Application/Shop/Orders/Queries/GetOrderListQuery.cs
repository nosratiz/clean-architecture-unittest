using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Orders.Queries
{
    public class GetOrderListQuery : IRequest<List<Order>>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public OrderStatus? OrderStatus { get; set; }

        public string Search { get; set; }
    }

    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<Order>>
    {
        private readonly ISolicoDbContext _context;

        public GetOrderListQueryHandler(ISolicoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            request.FromDate ??= new DateTime();
            request.ToDate ??= DateTime.Now;

            IQueryable<Order> orders = _context.Orders
                .OrderByDescending(x => x.CreateDate)
                .Where(x => x.CreateDate >= request.FromDate && x.CreateDate <= request.ToDate);


            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                orders = orders.Where(x =>
                    x.OrderNumber.Contains(request.Search) || x.DeliveryName.Contains(request.Search));
            }

            if (request.OrderStatus.HasValue)
            {
                orders = orders.Where(x => x.OrderStatus == (int)request.OrderStatus);
            }

            return await orders.ToListAsync(cancellationToken);
        }
    }
}