using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Messages.Command.DeleteCustomerMessage
{
    public class DeleteCustomerMessageCommandHandler : IRequestHandler<DeleteCustomerMessageCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ICurrentCustomerService _currentCustomerService;
        private readonly ILocalization _localization;

        public DeleteCustomerMessageCommandHandler(ISolicoDbContext context,
            ICurrentCustomerService currentCustomerService, ILocalization localization)
        {
            _context = context;
            _currentCustomerService = currentCustomerService;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteCustomerMessageCommand request, CancellationToken cancellationToken)
        {
            var userMessage = await GetUserMessageAsync(request, cancellationToken);

            if (userMessage is null)
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(await
                    _localization.GetMessage(ResponseMessage.MessageNotFound, cancellationToken))));
            }

            if (userMessage.CustomerId != Guid.Parse(_currentCustomerService.Id))
            {
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(
                    await _localization.GetMessage(ResponseMessage.deleteMessageNotAllowed, cancellationToken))));
            }

            userMessage.CustomerHasDeleted = true;

            var messageIds = await GetChildMessageIdListAsync(cancellationToken, userMessage);

            await DeleteChildMessageAsync(cancellationToken, messageIds);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

    
        #region Query
        private async Task<UserMessage> GetUserMessageAsync(DeleteCustomerMessageCommand request, CancellationToken cancellationToken)
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

                ChildUserMessages.ForEach(m => { m.CustomerHasDeleted = true; });
            }
        }

        private async Task<List<Guid>> GetChildMessageIdListAsync(CancellationToken cancellationToken, UserMessage userMessage)
        {
            return await _context.messages
                .Where(x => x.ParentId == userMessage.MessageId)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
        }

        #endregion
        
    }
}