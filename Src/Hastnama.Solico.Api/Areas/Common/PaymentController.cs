using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Payments.Command.CreatePayment;
using Hastnama.Solico.Application.Shop.Payments.Command.VerifyPayment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Api.Areas.Common
{
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PayOrder(CreatePaymentCommand createPaymentCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createPaymentCommand, cancellationToken);

            return result.ApiResult;
        }



        [HttpPost("VerifyPayment")]
        public async Task<IActionResult> VerifyPayment(VerifyPaymentCommand verifyPaymentCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(verifyPaymentCommand, cancellationToken);

            if (result.Success==false)
            {
                return result.ApiResult;
            }

            return NoContent();
        }
    }
}