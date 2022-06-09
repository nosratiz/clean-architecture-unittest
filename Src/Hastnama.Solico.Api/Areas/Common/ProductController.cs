using Hastnama.Solico.Application.Shop.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;

namespace Hastnama.Solico.Api.Areas.Common
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetProductForCustomerQuery getProductPagedListQuery,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(getProductPagedListQuery, cancellationToken));


        [HttpGet("{id}")]
        public async Task<IActionResult> GetInfo(long id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductQuery { Id = id }, cancellationToken);

            return result.ApiResult;
        }


        [HttpGet("MostVisitedProduct")]
        public async Task<IActionResult> GetMostVisitedProductByCustomer([FromQuery] int limit,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetMostVisitedProductForCustomerQuery { Limit = limit }, cancellationToken));


        [HttpGet("NewProductList")]
        public async Task<IActionResult> GetNewProductList([FromQuery] int limit, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetNewProductForCustomerQuery { limit = limit }, cancellationToken));


        [HttpGet("GetProductBasedOnOrder")]
        public async Task<IActionResult> GetProductBasedOnOrder([FromQuery] int limit,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetSimilarProductFromOrderQuery { Limit = limit }, cancellationToken));


        [HttpGet("{id}/similarProduct")]
        public async Task<IActionResult> GetSimilarProductAsync(long id, [FromQuery] int limit,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSimilarProductQuery { Id = id, limit = limit }, cancellationToken);

            return result.ApiResult;
        }
    }
}