using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Companies.Command.Create;
using Hastnama.Solico.Application.Shop.Companies.Command.Delete;
using Hastnama.Solico.Application.Shop.Companies.Command.Update;
using Hastnama.Solico.Application.Shop.Companies.Queires;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    [Authorize]
    public class CompanyController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions, CancellationToken cancellation)
        => Ok(await _mediator.Send(new GetCompanyPagedListQuery
        {
            Page = pagingOptions.Page,
            Limit = pagingOptions.Limit,
            Search = pagingOptions.Search
        }, cancellation));


        [HttpGet("{id}", Name = "GetCompanyInfo")]
        public async Task<IActionResult> GetInfo(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new GetCompanyQuery { Id = id }, cancellation);

            return result.ApiResult;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList(CancellationToken cancellation) => Ok(await _mediator.Send(new GetCompanyListQuery(), cancellation));

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyCommand createCompanyCommand, CancellationToken cancellation)
        {
            var result = await _mediator.Send(createCompanyCommand, cancellation);

            if (result.Success == false)
                return result.ApiResult;


            return CreatedAtAction(nameof(GetInfo), new {id=result.Data.Id}, result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateCompanyCommand updateCompanyCommand, CancellationToken cancellation)
        {
            var result = await _mediator.Send(updateCompanyCommand, cancellation);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new DeleteCompanyCommand { Id = id }, cancellation);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}