using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.CustomerConsults.Command.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    [Authorize]
    public class CustomerConsultController : BaseController
    {
        private readonly IMediator _mediator;

        public CustomerConsultController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerConsultCommand createCustomerConsultCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createCustomerConsultCommand, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}