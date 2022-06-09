using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Messages.Command.ReplyCustomerMessage
{
    public class ReplyCustomerMessageCommandHandler : IRequestHandler<ReplyCustomerMessageCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;
        private readonly ICurrentCustomerService _currentCustomerService;

        public ReplyCustomerMessageCommandHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization,
            ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<Result> Handle(ReplyCustomerMessageCommand request, CancellationToken cancellationToken)
        {
            var userMessage = await GetUserMessageAsync(request, cancellationToken);

            if (userMessage is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.MessageNotFound,
                        cancellationToken))));
            }

            if (userMessage.CustomerId != Guid.Parse(_currentCustomerService.Id))
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.AccessMessageDenied,
                        cancellationToken))));
            }

            var ReplyMessage = _mapper.Map<Message>(request);

            ReplyMessage.Title = userMessage.Message.Title;

            await _context.messages.AddAsync(ReplyMessage, cancellationToken);

            var setting = await _context.AppSettings.FirstOrDefaultAsync(cancellationToken);

            await _context.UserMessages.AddAsync(new UserMessage
            {
                UserId = setting.UserId,
                CustomerId = userMessage.CustomerId,
                SendDate = DateTime.Now,
                Message = ReplyMessage,
                IsAdmin =false
            }, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }

        #region Query

        private async Task<UserMessage> GetUserMessageAsync(ReplyCustomerMessageCommand request, CancellationToken cancellationToken)
        {
            return await _context.UserMessages
                .Include(x => x.Message)
                .SingleOrDefaultAsync(x => x.MessageId == request.ParentMessageId, cancellationToken);
        }

        #endregion
        
    }
}