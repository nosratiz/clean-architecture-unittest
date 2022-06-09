using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Interfaces.Statistic;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.UserManagement.Users.Command.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public DeleteUserCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.Ids)
            {
                var user = await GetUserAsync(id, cancellationToken);

                if (user is null)
                    return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.UserNotFound, cancellationToken))));


                if (user.RoleId == Role.Admin)
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.CanNotDeleteAdmin, cancellationToken))));

                user.IsDeleted = true;

                // await _statisticService.UpdateUserCount(false, cancellationToken);
            }


            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();

        }

        private async Task<User> GetUserAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}