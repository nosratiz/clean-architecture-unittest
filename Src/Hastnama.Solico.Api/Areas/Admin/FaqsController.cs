using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Faqs.Command.Create;
using Hastnama.Solico.Application.Cms.Faqs.Command.Delete;
using Hastnama.Solico.Application.Cms.Faqs.Command.Update;
using Hastnama.Solico.Application.Cms.Faqs.Queries;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{

    public class FaqsController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public FaqsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetFaqPagedListQuery
            {
                Limit = pagingOptions.Limit,
                Page = pagingOptions.Page,
                Search = pagingOptions.Search
            }, cancellationToken));


        [HttpGet("{id}", Name = "GetFaqInfo")]
        public async Task<IActionResult> GetFaqInfo(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetFaqQuery { Id = id }, cancellationToken);

            return result.ApiResult;
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateFaqCommand createFaqCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createFaqCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return CreatedAtAction(nameof(GetFaqInfo), new {id=result.Data.Id}, result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateFaqCommand updateFaqCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateFaqCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }



        [HttpPut("Delete")]
        public async Task<IActionResult> Delete(DeleteFaqCommand deleteFaqCommand, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(deleteFaqCommand, cancellationToken);


            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}