using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.User;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Messages.Command.ReplyUserMessage
{
    public class ReplyUserMessageCommandHandler : IRequestHandler<ReplyUserMessageCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public ReplyUserMessageCommandHandler(ISolicoDbContext context, ICurrentUserService currentUserService,
            IMapper mapper, ILocalization localization)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result> Handle(ReplyUserMessageCommand request, CancellationToken cancellationToken)
        {
            var userMessage = await GetUserMessageAsync(request, cancellationToken);

            if (userMessage is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.MessageNotFound,
                        cancellationToken))));
            }

            var ReplyMessage = _mapper.Map<Message>(request);

            ReplyMessage.Title = userMessage.Message.Title;
            

            await _context.messages.AddAsync(ReplyMessage, cancellationToken);
              
            await _context.UserMessages.AddAsync(new UserMessage
            {
                UserId = long.Parse(_currentUserService.UserId),
                CustomerId = userMessage.CustomerId,
                SendDate = DateTime.Now,
                Message = ReplyMessage,
                IsAdmin = true
            }, cancellationToken);

            await _context.SaveAsync(cancellationToken);
            
            return Result.SuccessFul();
        }

        #region Query

        private async Task<UserMessage> GetUserMessageAsync(ReplyUserMessageCommand request, CancellationToken cancellationToken)
        {
            return await _context.UserMessages
                .Include(x=>x.Message)
                .SingleOrDefaultAsync(x => x.MessageId == request.ParentMessageId, cancellationToken);
        }

        #endregion
        
    }
}