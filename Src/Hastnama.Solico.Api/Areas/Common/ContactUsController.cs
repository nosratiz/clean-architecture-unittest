using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.ContactUses.Command.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    public class ContactUsController : BaseController
    {
        private readonly IMediator _mediator;

        public ContactUsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateContactUsCommand createContactUsCommand,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createContactUsCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
