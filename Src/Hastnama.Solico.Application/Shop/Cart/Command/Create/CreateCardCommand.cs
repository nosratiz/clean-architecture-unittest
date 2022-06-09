using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Cart.Command.Create
{
    public class CreateCardCommand : IRequest<Result>
    {
        public long ProductId { get; set; }
        public int Count { get; set; }

        
    }
}