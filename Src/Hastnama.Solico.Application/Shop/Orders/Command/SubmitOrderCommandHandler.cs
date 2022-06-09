using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Orders.Dto;
using Hastnama.Solico.Domain.Models.UserManagement;

namespace Hastnama.Solico.Application.Shop.Orders.Command
{
    public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, Result<OrderDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISolicoWebService _solicoWebService;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly ILocalization _localization;

        public SubmitOrderCommandHandler(ISolicoDbContext context, IMapper mapper, ISolicoWebService solicoWebService,
            ICurrentCustomerService currentCustomerService, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _solicoWebService = solicoWebService;
            _currentCustomerService = currentCustomerService;
            _localization = localization;
        }

        public async Task<Result<OrderDto>> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
        {
            var orderItems = await GetItemInBasketAsync(cancellationToken);
            
            if (orderItems.Any() == false)
            {
                return Result<OrderDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.BasketEmpty, cancellationToken))));
            }

            var customer = await GetCustomerAsync(cancellationToken);

            var order = _mapper.Map<Order>(request);
            order.Address = customer.Address;
            order.DeliveryName = customer.FullName;
            order.FinalAmount = orderItems.Sum(x => x.Count * x.Price);
            order.SolicoCustomerId = _currentCustomerService.CustomerId;

         
            orderItems.ForEach(item => { item.Order = order; });

            await _context.Orders.AddAsync(order, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<OrderDto>.SuccessFul(_mapper.Map<OrderDto>(order));
        }

        #region Service

        private static List<QuotationItem> AddQuotationItem(List<OrderItem> OrderItems)
        {
            List<QuotationItem> quotationItems = new List<QuotationItem>();

            int count = 1;
            foreach (var item in OrderItems)
            {
                quotationItems.Add(new QuotationItem
                {
                    ITM_NUMBER = $"0000{count * 10}",
                    MATERIAL = item.Product.MaterialId,
                    TARGET_QU = item.Product.Unit,
                    TARGET_QTY = item.Count.ToString()
                });
                count++;
            }

            return quotationItems;
        }

        private async Task<Customer> GetCustomerAsync(CancellationToken cancellationToken)
        {
            return await _context.Customers.SingleOrDefaultAsync(x => x.Id == Guid.Parse(_currentCustomerService.Id),
                cancellationToken);
        }

        private async Task<List<OrderItem>> GetItemInBasketAsync(CancellationToken cancellationToken)
        {
            return await _context.OrderItems
                .Include(x => x.Product)
                .Where(x => x.CustomerId == Guid.Parse(_currentCustomerService.Id) && x.OrderId == null)
                .ToListAsync(cancellationToken);
        }   

        #endregion
     
    }
}