using System.Collections.Generic;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Users.Command.DeleteUser
{
    public class DeleteUserCommand : IRequest<Result>
    {
        public List<long> Ids { get; set; }
    }
}