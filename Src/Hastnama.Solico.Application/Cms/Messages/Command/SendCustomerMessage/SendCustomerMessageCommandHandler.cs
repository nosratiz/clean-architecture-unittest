using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Messages.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Claims.User;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Messages.Command.SendCustomerMessage
{
    public class SendCustomerMessageCommandHandler : IRequestHandler<SendCustomerMessageCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public SendCustomerMessageCommandHandler(ISolicoDbContext context, IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(SendCustomerMessageCommand request,
            CancellationToken cancellationToken)
        {
            var message = _mapper.Map<Message>(request);

            await _context.messages.AddAsync(message, cancellationToken);

            await _context.UserMessages.AddAsync(new UserMessage
            {
                Message = message,
                UserId = long.Parse(_currentUserService.UserId),
                CustomerId = request.CustomerId,
                SendDate = DateTime.Now,
                IsAdmin = true
            }, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}