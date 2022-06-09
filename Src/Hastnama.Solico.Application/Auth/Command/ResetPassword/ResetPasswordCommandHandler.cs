using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Auth.Command.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public ResetPasswordCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(request, cancellationToken);

            if (user is null)
                return Result.Failed(new NotFoundObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound, cancellationToken))));

            if (user.ExpiredVerification < DateTime.Now)
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ActiveCodeExpired,
                        cancellationToken))));


            user.Password = PasswordManagement.HashPass(request.NewPassword);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query

        private async Task<User> GetUserAsync(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(
                x => x.ActivationCode == request.ActiveCode && x.Email == request.Email, cancellationToken);
        }

        #endregion
      
    }
}