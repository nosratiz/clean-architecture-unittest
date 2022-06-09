using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Email;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Options;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hastnama.Solico.Application.Auth.Command.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;
        private readonly IEmailServices _emailServices;
        private readonly IOptionsMonitor<HostAddress> _hostAddress;

        public ForgetPasswordCommandHandler(ISolicoDbContext context, ILocalization localization,
            IEmailServices emailServices, IOptionsMonitor<HostAddress> hostAddress)
        {
            _context = context;
            _localization = localization;
            _emailServices = emailServices;
            _hostAddress = hostAddress;
        }

        public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(request, cancellationToken);

            if (user is null)
                return Result.SuccessFul(new OkObjectResult(new ApiMessage(
                    await _localization.GetMessage(ResponseMessage.EmailSuccessfullySent, cancellationToken))));

            user.ActivationCode = new Random().Next(111111, 999999).ToString();
            user.ExpiredVerification = DateTime.Now.AddDays(1);
            await _context.SaveAsync(cancellationToken);

            await _emailServices.SendMessage(request.Email,
                await _localization.GetMessage(ResponseMessage.ForgotPassword, cancellationToken),
                $"{_hostAddress.CurrentValue.BaseUrl}/reset-password?activeCode={user.ActivationCode}&email={user.Email}");


            return Result.SuccessFul(new OkObjectResult(
                new ApiMessage(await _localization.GetMessage(ResponseMessage.EmailSuccessfullySent,
                    cancellationToken))));
        }

        #region Query

        private async Task<User> GetUserAsync(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        }

        #endregion
     
    }
}