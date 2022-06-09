using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Create;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Delete;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Update;
using Hastnama.Solico.Application.Cms.SlideShows.Queries;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{

    public class SlideShowsController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public SlideShowsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetSliderPagedListQuery
            {
                Limit = pagingOptions.Limit,
                Page = pagingOptions.Page,
                Search = pagingOptions.Search
            }, cancellationToken));

        [HttpGet("{id}", Name = "SlideShowInfo")]
        public async Task<IActionResult> GetSlideShow(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSlideShowQuery { Id = id }, cancellationToken);

            return result.ApiResult;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateSlidShowCommand createSlidShowCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createSlidShowCommand, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return CreatedAtAction(nameof(GetSlideShow), new {id=result.Data.Id}, result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateSlideShowCommand updateSlideShow, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateSlideShow, cancellationToken);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPut("Delete")]
        public async Task<IActionResult> Delete(DeleteSlideShowCommand deleteSlideShowCommand,
                CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(deleteSlideShowCommand, cancellationToken);


            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
