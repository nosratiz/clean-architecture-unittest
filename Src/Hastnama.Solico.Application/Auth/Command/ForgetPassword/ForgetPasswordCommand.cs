using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Auth.Command.ForgetPassword
{
    public class ForgetPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }
}