using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.UserManagement.Customers.Command.ChangePassword
{
    public class ChangePasswordCustomerCommand : IRequest<Result>
    {
        public string Password { get; set; }

        public string NewPassword { get; set; }
    }
}