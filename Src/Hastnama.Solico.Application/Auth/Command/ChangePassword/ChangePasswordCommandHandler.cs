using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.User;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Auth.Command.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILocalization _localization;

        public ChangePasswordCommandHandler(ISolicoDbContext context, ICurrentUserService currentUserService,
            ILocalization localization)
        {
            _context = context;
            _currentUserService = currentUserService;
            _localization = localization;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(cancellationToken);

            if (user is null)
                return Result.Failed(new NotFoundObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound, cancellationToken))));

            if (PasswordManagement.CheckPassword(request.CurrentPassword, user.Password) == false)
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.WrongPassword, cancellationToken))));


            user.Password = PasswordManagement.HashPass(request.NewPassword);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query

        private async Task<User> GetUserAsync(CancellationToken cancellationToken)
        {
            return await _context.Users
                .SingleOrDefaultAsync(x => x.Id == int.Parse(_currentUserService.UserId),
                    cancellationToken);
        }

        #endregion

    }
}