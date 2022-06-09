using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Payments.Command.CreatePayment
{
    public class CreatePaymentCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }

    }
}