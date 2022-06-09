using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Faqs.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    public class FaqsController : BaseController
    {
        private readonly IMediator _mediator;

        public FaqsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation) =>
            Ok(await _mediator.Send(new GetFaqListQuery(), cancellation));
    }
}
