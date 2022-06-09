using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Messages.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Messages.Queries
{
    public class GetConversationQuery : IRequest<List<UserMessageDto>>
    {
        public Guid MessageId { get; set; }
    }

    public class GetConversationQueryHandler : IRequestHandler<GetConversationQuery, List<UserMessageDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetConversationQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization, ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<List<UserMessageDto>> Handle(GetConversationQuery request, CancellationToken cancellationToken)
        {
            var userMessage = await _context.UserMessages
                .Include(x => x.Message)
                .Include(x => x.Customer)
                .Include(x => x.User)
                .Where(x => x.MessageId == request.MessageId ||
                            x.Message.ParentId == request.MessageId)
                .OrderByDescending(x => x.SendDate)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<UserMessageDto>>(userMessage);
        }
    }
}