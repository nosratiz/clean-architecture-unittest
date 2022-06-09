using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.Login
{
    public class CustomerLoginCommand : IRequest<Result<AuthResult>>
    {
        public string Mobile { get; set; }

        public string Passwords { get; set; }
    }
}