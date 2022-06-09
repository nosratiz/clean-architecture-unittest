using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Orders.Command;
using Hastnama.Solico.Application.Shop.Orders.Queries;
using Hastnama.Solico.Application.UserManagement.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.User
{
    public class OrdersController : UserBaseController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("MyOrder")]
        public async Task<IActionResult> GetMyOrder([FromQuery] GetMyOrderPagedListQuery getMyOrderPagedListQuery,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(getMyOrderPagedListQuery, cancellationToken));

        [HttpPost("SubmitOrder")]
        public async Task<IActionResult> SubmitOrder(SubmitOrderCommand submitOrderCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(submitOrderCommand, cancellationToken);


            return result.ApiResult;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderInfo(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOrderQuery {Id = id}, cancellationToken);

            return result.ApiResult;
        }


        [HttpGet("openItems")]
        public async Task<IActionResult> GetOpenItems([FromQuery] GetCustomerOpenItemsQuery getCustomerOpenItemsQuery,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(getCustomerOpenItemsQuery, cancellationToken));


        [HttpPost("{Id}/DuplicateOrder")]
        public async Task<IActionResult> DuplicateOrder(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DuplicateOrderCommand {Id = id}, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}