using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Payments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.User
{
    public class TransactionController : UserBaseController
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]GetBankTransactionPagedListQuery model,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(model, cancellationToken));
    }
}