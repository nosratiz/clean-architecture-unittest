using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Auth.Command.Login
{
    public class LoginCommand : IRequest<Result<AuthResult>>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}