using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Subscribers.Command.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    public class SubscribersController : BaseController
    {
        private readonly IMediator _mediator;

        public SubscribersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateSubscriberCommand createSubscriberCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createSubscriberCommand, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}