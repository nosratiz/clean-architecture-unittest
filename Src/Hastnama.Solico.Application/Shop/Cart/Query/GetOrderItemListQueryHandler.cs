using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Cart.Dto;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Helper.Claims;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Cart.Query
{
    public class GetOrderItemListQueryHandler : IRequestHandler<GetOrderItemListQuery, List<OrderItemDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetOrderItemListQueryHandler(ISolicoDbContext context, IMapper mapper,
            ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<List<OrderItemDto>> Handle(GetOrderItemListQuery request, CancellationToken cancellationToken)
        {
            var orderItems = await _context.OrderItems
                .Include(x => x.Product)
                .Where(x => x.CustomerId == Guid.Parse(_currentCustomerService.Id) && (x.OrderId == null ||
                            x.Order.OrderStatus == (int) OrderStatus.InBasket))
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<OrderItemDto>>(orderItems);
        }
    }
}