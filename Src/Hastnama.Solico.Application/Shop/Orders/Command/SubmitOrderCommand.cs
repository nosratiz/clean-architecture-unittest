using Hastnama.Solico.Application.Shop.Orders.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Orders.Command
{
    public class SubmitOrderCommand : IRequest<Result<OrderDto>>
    {
    }
}