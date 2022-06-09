using System;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        
        public string Mobile { get; set; }

        public string Password { get; set; }
    }
}