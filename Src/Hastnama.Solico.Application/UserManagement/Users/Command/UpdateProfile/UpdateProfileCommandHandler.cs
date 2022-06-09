using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Users.Command.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<User>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public UpdateProfileCommandHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<User>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(request, cancellationToken);

            if (user is null)
                return Result<User>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound, cancellationToken))));

            _mapper.Map(request, user);

            await _context.SaveAsync(cancellationToken);


            return Result<User>.SuccessFul(user);
        }

        #region Query

        private async Task<User> GetUserAsync(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id == request.Id , cancellationToken);
        }

        #endregion
      
    }
}