using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.ConfirmCode
{
    public class ConfirmCodeCommand : IRequest<Result<AuthResult>>
    {
        public string Mobile { get; set; }

        public string Code { get; set; }
    }
}