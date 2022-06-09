using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.CustomerConsults.Command.Delete;
using Hastnama.Solico.Application.Cms.CustomerConsults.Command.Settle;
using Hastnama.Solico.Application.Cms.CustomerConsults.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Admin
{
    public class CustomerConsultController : AdminBaseController
    {
        private readonly IMediator _mediator;

        public CustomerConsultController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetConsultPagedListQuery getConsultPagedListQuery,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(getConsultPagedListQuery, cancellationToken));



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteCustomerConsultCommand {Id = id}, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }


        [HttpPut("{id}/activation")]
        public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SettleCommand {Id = id}, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}