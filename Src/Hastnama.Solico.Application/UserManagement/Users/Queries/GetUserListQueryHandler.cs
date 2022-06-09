using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Users.ModelDto;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Users.Queries
{
    public class GetUserListQueryHandler : PagingService<User>, IRequestHandler<GetUserListQuery, PagedList<UserDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<User> users = _context.Users
                .Include(x => x.Role);

            if (!string.IsNullOrWhiteSpace(request.Search))
                users = users.Where(x => x.Email.Contains(request.Search.ToLower())
                                         || x.Name.Contains(request.Search.ToLower())
                                         || x.Family.Contains(request.Search.ToLower()));


            var userList = await GetPagedAsync(request.Page, request.Limit, users,cancellationToken);

            return userList.MapTo<UserDto>(_mapper);
        }
    }
}