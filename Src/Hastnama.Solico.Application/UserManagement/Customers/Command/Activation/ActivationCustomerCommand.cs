using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.Activation
{
    public class ActivationCustomerCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}