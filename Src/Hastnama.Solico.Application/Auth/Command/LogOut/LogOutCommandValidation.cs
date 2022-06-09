using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Extensions;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Auth.Command.LogOut
{
    public class LogOutCommandValidation : IRequestHandler<LogOutCommand,Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogOutCommandValidation(ISolicoDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext.GetAuthorizationToken();

            var userToken = await _context.UserTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

            if (userToken !=null)
            {
                userToken.IsUsed = true;
                await _context.SaveAsync(cancellationToken);
            }
            
            return Result.SuccessFul();
        }
    }
}