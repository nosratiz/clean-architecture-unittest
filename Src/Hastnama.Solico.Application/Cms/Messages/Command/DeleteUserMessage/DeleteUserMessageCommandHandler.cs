using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Hastnama.Solico.Application.Cms.Messages.Command.DeleteUserMessage
{
    public class DeleteUserMessageCommandHandler : IRequestHandler<DeleteUserMessageCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILocalization _localization;

        public DeleteUserMessageCommandHandler(ISolicoDbContext context, ICurrentUserService currentUserService,
            ILocalization localization)
        {
            _context = context;
            _currentUserService = currentUserService;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteUserMessageCommand request, CancellationToken cancellationToken)
        {
            var userMessage = await GetUserMessageAsync(request, cancellationToken);

            if (userMessage is null)
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(await
                    _localization.GetMessage(ResponseMessage.MessageNotFound, cancellationToken))));
            }

            if (userMessage.UserId != long.Parse(_currentUserService.UserId))
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(
                    await _localization.GetMessage(ResponseMessage.deleteMessageNotAllowed, cancellationToken))));
            }

            userMessage.UserHasDeleted = true;
            
            var messageIds = await GetChildMessageIdListAsync(cancellationToken, userMessage);

            await DeleteChildMessageAsync(cancellationToken, messageIds);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query
        private async Task<UserMessage> GetUserMessageAsync(DeleteUserMessageCommand request,
            CancellationToken cancellationToken)
        {
            return await _context.UserMessages
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        private async Task DeleteChildMessageAsync(CancellationToken cancellationToken, List<Guid> messageIds)
        {
            if (messageIds.Any())
            {
                var ChildUserMessages = await _context.UserMessages
                    .Where(x => messageIds.Contains(x.MessageId))
                    .ToListAsync(cancellationToken);


                ChildUserMessages.ForEach(m => { m.UserHasDeleted = true; });
            }
        }

        private async Task<List<Guid>> GetChildMessageIdListAsync(CancellationToken cancellationToken,
            UserMessage userMessage)
        {
            return await _context.messages
                .Where(x => x.ParentId == userMessage.MessageId)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
        }
        #endregion

       
    }
}