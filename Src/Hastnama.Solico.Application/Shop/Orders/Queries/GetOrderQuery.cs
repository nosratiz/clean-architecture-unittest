using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Cart.Dto;
using Hastnama.Solico.Application.Shop.Orders.Dto;
using Hastnama.Solico.Common.Extensions;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Orders.Queries
{
    public class GetOrderQuery : IRequest<Result<OrderDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<OrderDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetOrderQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await GetOrderAsync(request, cancellationToken);

            if (order is null)
            {
                return Result<OrderDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.OrderNotFound, cancellationToken))));
            }

            var orderDto = _mapper.Map<OrderDto>(order);

            var historyItem = orderDto.OrderStatusHistories
                .OrderBy(x => x.OrderStatus).FirstOrDefault();


            if (historyItem != null)
            {
                var newItemsIds = orderDto.OrderItems
                    .Select(x => x.Id)
                    .Except(historyItem.OrderItems.Select(x => x.Id))
                    .ToList();

                var removedItemsIds = historyItem.OrderItems
                    .Select(x => x.Id)
                    .Except(orderDto.OrderItems.Select(x => x.Id))
                    .ToList();


                if (removedItemsIds.Any())
                {
                    UpdateOrderItem(historyItem, removedItemsIds, orderDto);
                }

                UpdateHistoryItem(orderDto, newItemsIds, historyItem);
            }


            return Result<OrderDto>.SuccessFul(orderDto);
        }

        private static void UpdateHistoryItem(OrderDto orderDto, List<Guid> newItemsIds,
            OrderStatusHistoryDto historyItem)
        {
            
            var items = orderDto.OrderItems.Where(x => newItemsIds.Contains(x.Id)).ToList();
            
            var existItems = orderDto.OrderItems.Where(x => !newItemsIds.Contains(x.Id)).ToList();


            List<OrderItemDto> newItems = new List<OrderItemDto>();

            if (newItemsIds.Any())
            {
                foreach (var existItem in existItems)
                {
                    var existInCart = historyItem.OrderItems.FirstOrDefault(x => x.Id == existItem.Id);

                    newItems.Add(new OrderItemDto
                    {
                        Id = existItem.Id,
                        ProductId = existItem.ProductId,
                        Price = existInCart?.Price ?? 0,
                        ProductImage = existItem.ProductImage,
                        ProductName = existItem.ProductName,
                        Count = existInCart?.Count ?? 0,
                        CreateDate = existItem.CreateDate,
                    });
                }

                foreach (var item in items)
                {
                    newItems.Add(new OrderItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Price = item.Price,
                        ProductImage = item.ProductImage,
                        ProductName = item.ProductName,
                        Count = item.Count,
                        CreateDate = item.CreateDate
                    });
                }

                newItems.ForEach(x =>
                {
                    x.Count = 0;
                    x.Price = 0;
                });

            }
            else
            {
                var allItems = orderDto.OrderItems.ToList();
                
                foreach (var existItem in allItems)
                {
                    var existInCart = historyItem.OrderItems.FirstOrDefault(x => x.Id == existItem.Id);

                    newItems.Add(new OrderItemDto
                    {
                        Id = existItem.Id,
                        ProductId = existItem.ProductId,
                        Price = existInCart?.Price ?? 0,
                        ProductImage = existItem.ProductImage,
                        ProductName = existItem.ProductName,
                        Count = existInCart?.Count ?? 0,
                        CreateDate = existItem.CreateDate,
                    });
                }
            }
        
            historyItem.OrderItems = newItems;
        }

        #region Query

        private static void UpdateOrderItem(OrderStatusHistoryDto historyItem, List<Guid> removedItemsIds,
            OrderDto orderDto)
        {
            var items = historyItem.OrderItems.Where(x => removedItemsIds.Contains(x.Id)).ToList();
            List<OrderItemDto> newItems = new List<OrderItemDto>();

            foreach (var item in items)
            {
                newItems.Add(new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    ProductImage = item.ProductImage,
                    ProductName = item.ProductName,
                    Count = item.Count,
                    CreateDate = item.CreateDate
                });
            }

            newItems.ForEach(x =>
            {
                x.Count = 0;
                x.Price = 0;
            });

            orderDto.OrderItems.AddRange(newItems);
        }


        private async Task<Order> GetOrderAsync(GetOrderQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product).Include(x => x.OrderStatusHistories)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
    }
}