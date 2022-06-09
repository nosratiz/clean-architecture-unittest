using Hastnama.Solico.Common.Result;
using MediatR;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.SendConfirmCode
{
    public class SendConfirmCodeCommand : IRequest<Result>
    {
        public string Mobile { get; set; }
    }
}