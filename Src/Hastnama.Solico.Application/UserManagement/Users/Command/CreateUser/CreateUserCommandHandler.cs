using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Interfaces.Statistic;
using Hastnama.Solico.Application.UserManagement.Users.ModelDto;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;

namespace Hastnama.Solico.Application.UserManagement.Users.Command.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDailyStatistic _dailyStatistic;

        public CreateUserCommandHandler(ISolicoDbContext context, IMapper mapper, IDailyStatistic dailyStatistic)
        {
            _context = context;
            _mapper = mapper;
            _dailyStatistic = dailyStatistic;
        }

        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            await _context.Users.AddAsync(user, cancellationToken);

            // await _statisticService.UpdateUserCount(true, cancellationToken);
            //
            // await _dailyStatistic.UpdateRegisterUser(cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<UserDto>.SuccessFul(_mapper.Map<UserDto>(user));


        }
    }
}