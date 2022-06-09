using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Subscribers.Command.Delete;
using Hastnama.Solico.Application.Cms.Subscribers.Queries;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class SubscribersController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public SubscribersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetSubscriberPagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Search = pagingOptions.Search
            }, cancellationToken));


        [HttpPut("Delete")]
        public async Task<IActionResult> Delete(DeleteSubscriberCommand deleteSubscriberCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(deleteSubscriberCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;


            return NoContent();
        }
    }
}