using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    public class ProductCategoryController : BaseController
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetSiteProductCategoryListQuery(), cancellationToken));
    }
}
