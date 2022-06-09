using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.UserManagement.Users.ModelDto;
using Hastnama.Solico.Common.Helper.Claims;
using Hastnama.Solico.Common.Helper.Claims.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Users.Queries
{
    public class GetProfileQuery: IRequest<UserDto>
    {
        
    }
    
    public class GetProfileQueryHandler :IRequestHandler<GetProfileQuery,UserDto>
    {
        private readonly ISolicoDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetProfileQueryHandler(ISolicoDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == int.Parse(_currentUserService.UserId), cancellationToken);
            
            return _mapper.Map<UserDto>(user);
        }
    }
}