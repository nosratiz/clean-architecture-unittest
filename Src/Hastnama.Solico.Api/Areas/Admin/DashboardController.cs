using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class DashboardController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
            Ok(await _mediator.Send(new GetDashboardQuery(), cancellationToken));
    }
}