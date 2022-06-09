using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Orders.Dto;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Shop.Orders.Queries
{
    public class GetMyOrderPagedListQuery : PagingOptions, IRequest<PagedList<OrderDto>>
    {
        public bool? IsPaid { get; set; }
    }
    
    public class GetMyOrderPagedListQueryHandler : PagingService<Order>, IRequestHandler<GetMyOrderPagedListQuery,PagedList<OrderDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetMyOrderPagedListQueryHandler(ISolicoDbContext context, IMapper mapper, ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<PagedList<OrderDto>> Handle(GetMyOrderPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Order> orders =  _context.Orders.OrderByDescending(x=>x.CreateDate).Where(x =>
                x.OrderItems.Any(o => o.CustomerId == Guid.Parse(_currentCustomerService.Id)));

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                orders = orders.Where(x =>
                    x.OrderNumber.Contains(request.Search) || x.QuotationNumber.Contains(request.Search));
            }

            if (request.IsPaid.HasValue)
            {
                if (request.IsPaid.Value==true)
                {
                    orders = orders.Where(x => x.OrderStatus==(int) OrderStatus.Delivered);
                }
                else
                {
                    orders = orders.Where(x => x.OrderStatus!=(int) OrderStatus.Delivered);
                }
                
              
            }


            var orderList = await GetPagedAsync(request.Page, request.Limit, orders, cancellationToken);

            return orderList.MapTo<OrderDto>(_mapper);
        }
    }
}