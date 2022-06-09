using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Auth.Command.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string ActiveCode { get; set; }
        public string NewPassword { get; set; }
    }
}