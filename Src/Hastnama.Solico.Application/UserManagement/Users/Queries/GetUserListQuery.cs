using Hastnama.Solico.Application.UserManagement.Users.ModelDto;
using Hastnama.Solico.Common.Helper.Pagination;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Users.Queries
{
    public class GetUserListQuery : PagingOptions, IRequest<PagedList<UserDto>>
    {

    }

}