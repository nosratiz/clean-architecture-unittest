using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.ContactUses.Command.Delete;
using Hastnama.Solico.Application.Cms.ContactUses.Queries;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{

    public class ContactUsController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public ContactUsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetContactUsPagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Search = pagingOptions.Search
            }, cancellationToken));



        [HttpGet("{id}")]
        public async Task<IActionResult> GetInfo(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetContactUsQuery { Id = id }, cancellationToken);

            return result.ApiResult;
        }


       [HttpPut("Delete")]
        public async Task<IActionResult> Delete(DeleteContactUsCommand deleteContactUsCommand,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(deleteContactUsCommand, cancellationToken);

            if (result.Success==false)
                return result.ApiResult;

            return NoContent();
        }
    }
} 