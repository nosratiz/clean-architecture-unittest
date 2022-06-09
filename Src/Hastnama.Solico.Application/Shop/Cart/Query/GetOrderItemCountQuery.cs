using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Shop.Cart.Query
{
    public class GetOrderItemCountQuery : IRequest<int>
    {
        
    }
    
    public class GetOrderItemCountQueryHandler : IRequestHandler<GetOrderItemCountQuery,int>
    {
        private readonly ISolicoDbContext _context;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetOrderItemCountQueryHandler(ISolicoDbContext context, ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<int> Handle(GetOrderItemCountQuery request, CancellationToken cancellationToken)
        {
            return await _context.OrderItems.CountAsync
            (x => x.CustomerId == Guid.Parse(_currentCustomerService.Id) && (x.OrderId == null || x.Order.OrderStatus == (int)OrderStatus.ReadyForPay),
                cancellationToken: cancellationToken);
        }
    }
}