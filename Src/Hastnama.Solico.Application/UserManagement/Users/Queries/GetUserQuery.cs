using Hastnama.Solico.Application.UserManagement.Users.ModelDto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Users.Queries
{
    public class GetUserQuery : IRequest<Result<UserDto>>
    {
        public long Id { get; set; }
    }
}