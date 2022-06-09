using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Auth.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResult>>
    {
        private readonly ISolicoDbContext _context;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILocalization _localization;

        public LoginCommandHandler(ISolicoDbContext context, ITokenGenerator tokenGenerator, ILocalization localization)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _localization = localization;
        }

        public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(request.UserName, cancellationToken);

            if (user is null)
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.InvalidUserOrPassword,
                        cancellationToken))));

            if (PasswordManagement.CheckPassword(request.Password, user.Password) == false)
                return Result<AuthResult>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.InvalidUserOrPassword,
                        cancellationToken))));


            var tokenResponse = await _tokenGenerator.Generate(user, cancellationToken);

            return Result<AuthResult>.SuccessFul(tokenResponse);
        }

        #region Qauery

        private async Task<User> GetUserAsync(string userName, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.PhoneNumber == userName,
                    cancellationToken);
        }

        #endregion
    }
}