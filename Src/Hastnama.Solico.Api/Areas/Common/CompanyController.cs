using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Companies.Queires;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    [Authorize]
    public class CompanyController : BaseController
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
            Ok(await _mediator.Send(new GetCompanyListQuery(), cancellationToken));
    }
}