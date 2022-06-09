using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Cart.Command.Create;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Cart.Command.Update
{
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly IMapper _mapper;

        public UpdateCartCommandHandler(ISolicoDbContext context, ILocalization localization,
            ICurrentCustomerService currentCustomerService, IMapper mapper)
        {
            _context = context;
            _localization = localization;
            _currentCustomerService = currentCustomerService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var product = await GetProductAsync(request, cancellationToken);

            if (product is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ProductNotFound,
                        cancellationToken))));
            }

            var orderItems = await GetCustomerItemsInBasketAsync(cancellationToken);

            var existItemInBasket = orderItems.FirstOrDefault(x => x.ProductId == request.ProductId);

            if (existItemInBasket != null)
            {
                existItemInBasket.Count = request.Count;
            }
            else
            {
                var orderItem = _mapper.Map<OrderItem>(request);

                orderItem.CustomerId = Guid.Parse(_currentCustomerService.Id);

                orderItem.Price = product.CustomerProductPrices
                    .FirstOrDefault(x => x.ProductId == product.Id)
                    ?.Price ?? 0;


                await _context.OrderItems.AddAsync(orderItem, cancellationToken);
            }


            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query

        private async Task<List<OrderItem>> GetCustomerItemsInBasketAsync(CancellationToken cancellationToken)
        {
            return await _context.OrderItems
                .Include(x => x.Product)
                .Where(x => (x.CustomerId == Guid.Parse(_currentCustomerService.Id) && x.OrderId == null) ||
                            x.Order.OrderStatus == (int) OrderStatus.InBasket)
                .ToListAsync(cancellationToken);
        }

        private async Task<Product> GetProductAsync(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(x => x.CustomerProductPrices)
                .SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);
        }

        #endregion
    }
}