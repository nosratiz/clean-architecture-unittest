using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.SlideShows.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    public class SlideShowsController : BaseController
    {
        private readonly IMediator _mediator;

        public SlideShowsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        => Ok(await _mediator.Send(new GetSliderListQuery(), cancellation));
    }
}