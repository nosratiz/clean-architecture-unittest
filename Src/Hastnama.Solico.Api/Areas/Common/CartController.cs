using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Cart.Command.Create;
using Hastnama.Solico.Application.Shop.Cart.Command.Delete;
using Hastnama.Solico.Application.Shop.Cart.Command.Update;
using Hastnama.Solico.Application.Shop.Cart.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
            Ok(await _mediator.Send(new GetOrderItemListQuery(), cancellationToken));


        [HttpGet("count")]
        public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOrderItemCountQuery(), cancellationToken);

            return Ok(new { count = result });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteOrderItemCommand { Id = id }, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(CreateCardCommand createCardCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createCardCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateCart(UpdateCartCommand updateCartCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateCartCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}