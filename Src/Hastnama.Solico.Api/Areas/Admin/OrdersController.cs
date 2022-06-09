using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Excel;
using Hastnama.Solico.Application.Shop.Orders.Command;
using Hastnama.Solico.Application.Shop.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class OrdersController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetOrderPagedListQuery getOrderPagedListQuery,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(getOrderPagedListQuery, cancellationToken));


        [HttpGet("ExcelReport")]
        public async Task<IActionResult> GetExcelReport([FromQuery] GetOrderListQuery getOrderListQuery,
            CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(getOrderListQuery, cancellationToken);

            var url = ReportGenerator.OrderList(orders);

            return Ok(new ReportDto { Url = url });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderInfo(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOrderQuery { Id = id }, cancellationToken);

            return result.ApiResult;
        }


        [HttpGet("{id}/InquireSailedOrder")]
        public async Task<IActionResult> GetSailedOrder(Guid Id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSailedOrderQuery { Id = Id }, cancellationToken);

            return result.ApiResult;
        }

        [HttpGet("{id}/Invoice")]
        public async Task<IActionResult> GetInvoice(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetInvoiceQuery { Id = id }, cancellationToken);

            return result.ApiResult;
        }


        [HttpPut("{id}/CancelOrder")]
        public async Task<IActionResult> CancelOrderAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CancelOrderCommand {Id = id}, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}