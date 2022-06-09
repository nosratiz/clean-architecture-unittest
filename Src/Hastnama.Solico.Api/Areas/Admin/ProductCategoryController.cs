using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Create;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Delete;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Update;
using Hastnama.Solico.Application.Shop.ProductCategories.Queries;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class ProductCategoryController : AdminBaseController
    {

        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions, CancellationToken cancellation)
        => Ok(await _mediator.Send(new GetProductCategoryPagedListQuery
        {
            Page = pagingOptions.Page,
            Limit = pagingOptions.Limit,
            Search = pagingOptions.Search
        }, cancellation));


        [HttpPost("addChildren")]
        public async Task<IActionResult> Create(CreateChildrenCategoryCommand childrenCategoryCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(childrenCategoryCommand, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpGet("{id}", Name = "GetProductCategoryInfo")]
        public async Task<IActionResult> GetProductCategoryInfo(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetProductCategoryQuery { Id = id }, cancellation);

            return result.ApiResult;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCategoryCommand createProductCategoryCommand, CancellationToken cancellation)
        {
            var result = await _mediator.Send(createProductCategoryCommand, cancellation);

            if (result.Success == false)
                return result.ApiResult;


            return CreatedAtAction(nameof(GetProductCategoryInfo), new {id=result.Data.Id}, result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductCategoryCommand updateProductCategory, CancellationToken cancellation)
        {
            var result = await _mediator.Send(updateProductCategory, cancellation);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


      [HttpPut("Delete")]
        public async Task<IActionResult> Delete(DeleteProductCategoryCommand deleteProductCategoryCommand, CancellationToken cancellation)
        {
            var result = await _mediator.Send(deleteProductCategoryCommand, cancellation);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] string search, CancellationToken cancellation) 
            => Ok(await _mediator.Send(new GetProductCategoryListQuery{Search = search}, cancellation));
    }
}