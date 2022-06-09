using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Payments.Command.VerifyPayment
{
    public class VerifyPaymentCommand : IRequest<Result>
    {
        public string Token { get; set; }
    }
}