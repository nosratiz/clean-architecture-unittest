using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.HtmlParts.Command.Create;
using Hastnama.Solico.Application.Cms.HtmlParts.Command.Update;
using Hastnama.Solico.Application.Cms.HtmlParts.Queries;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class HtmlPartController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public HtmlPartController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetHtmlPartPagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Search = pagingOptions.Search
            }, cancellationToken);


            return Ok(result);
        }


        [HttpGet("{id}", Name = "htmlPartInfo")]
        public async Task<IActionResult> GetInfo(long id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetHtmlPartQuery {Id = id}, cancellationToken);

            return result.ApiResult;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHtmlPartCommand createHtmlPartCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createHtmlPartCommand, cancellationToken);

            if (result.Success == false)
            {
                return result.ApiResult;
            }

            return CreatedAtAction(nameof(GetInfo), new {id=result.Data.Id}, result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateHtmlPartCommand updateHtmlPartCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateHtmlPartCommand, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        
    }
}