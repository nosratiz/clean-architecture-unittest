using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Orders.Dto;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Orders.Queries
{
    public class GetOrderPagedListQuery : PagingOptions, IRequest<PagedList<OrderDto>>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public OrderStatus? OrderStatus { get; set; }
    }

    public class GetOrderPagedListQueryHandler : PagingService<Order>,
        IRequestHandler<GetOrderPagedListQuery, PagedList<OrderDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetOrderPagedListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<OrderDto>> Handle(GetOrderPagedListQuery request,
            CancellationToken cancellationToken)
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
                orders = orders.Where(x => x.OrderStatus == (int) request.OrderStatus);
            }

            var orderList = await GetPagedAsync(request.Page, request.Limit, orders, cancellationToken);

            return orderList.MapTo<OrderDto>(_mapper);
        }
    }
}