using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Messages.Command.SendUserMessage
{
    public class SendUserMessageCommandHandler : IRequestHandler<SendUserMessageCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public SendUserMessageCommandHandler(ISolicoDbContext context, IMapper mapper,
            ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<Result> Handle(SendUserMessageCommand request, CancellationToken cancellationToken)
        {
            var message = _mapper.Map<Message>(request);

            await _context.messages.AddAsync(message, cancellationToken);

            var setting = await _context.AppSettings.FirstOrDefaultAsync(cancellationToken);
            
            await _context.UserMessages.AddAsync(new UserMessage
            {
                UserId = setting.UserId,
                CustomerId = Guid.Parse(_currentCustomerService.Id),
                SendDate = DateTime.Now,
                IsAdmin = false,
                Message = message
            }, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}