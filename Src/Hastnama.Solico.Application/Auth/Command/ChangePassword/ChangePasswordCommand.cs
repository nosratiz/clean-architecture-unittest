using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Auth.Command.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Result>
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}