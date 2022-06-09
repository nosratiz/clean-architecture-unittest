using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Cart.Command.Delete
{
    public class DeleteOrderItemCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}