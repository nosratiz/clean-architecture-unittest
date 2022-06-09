using System.Collections.Generic;
using Hastnama.Solico.Application.Shop.Cart.Dto;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Cart.Query
{
    public class GetOrderItemListQuery : IRequest<List<OrderItemDto>>
    {
        
    }
}