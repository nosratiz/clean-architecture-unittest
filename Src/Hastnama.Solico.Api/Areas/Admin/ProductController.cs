using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Excel;
using Hastnama.Solico.Application.Shop.Products.Command.Update;
using Hastnama.Solico.Application.Shop.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class ProductController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetProductPagedListQuery getProductPagedListQuery,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(getProductPagedListQuery, cancellationToken));


        [HttpGet("ExcelReport")]
        public async Task<IActionResult> GetExcelReport([FromQuery] GetProductListQuery getProductListQuery,
            CancellationToken cancellationToken)
        {
            var products = await _mediator.Send(getProductListQuery, cancellationToken);

            var url = ReportGenerator.ProductList(products);

            return Ok(new ReportDto { Url = url });
        }


        [HttpGet("{id}", Name = "ProductInfo")]
        public async Task<IActionResult> GetInfo(long id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAdminProductQuery { Id = id }, cancellationToken);

            return result.ApiResult;
        }


       


        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductCommand updateProductCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateProductCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
        
    }
}