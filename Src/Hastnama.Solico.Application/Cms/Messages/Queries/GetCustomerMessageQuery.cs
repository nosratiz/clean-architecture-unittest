using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Messages.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Claims.Customer;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotImplementedException = System.NotImplementedException;

namespace Hastnama.Solico.Application.Cms.Messages.Queries
{
    public class GetCustomerMessageQuery : PagingOptions, IRequest<PagedList<UserMessageDto>>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    public class GetCustomerMessageQueryHandler : PagingService<UserMessage>,
        IRequestHandler<GetCustomerMessageQuery, PagedList<UserMessageDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentCustomerService _currentCustomerService;

        public GetCustomerMessageQueryHandler(ISolicoDbContext context, IMapper mapper,
            ICurrentCustomerService currentCustomerService)
        {
            _context = context;
            _mapper = mapper;
            _currentCustomerService = currentCustomerService;
        }

        public async Task<PagedList<UserMessageDto>> Handle(GetCustomerMessageQuery request,
            CancellationToken cancellationToken)
        {
            request.FromDate ??= new DateTime();
            request.ToDate ??= DateTime.Now;

            IQueryable<UserMessage> messages =
                _context.UserMessages.OrderByDescending(x => x.SendDate)
                    .Where(x =>
                        x.CustomerId == Guid.Parse(_currentCustomerService.Id) && x.CustomerHasDeleted == false &&
                        x.Message.ParentId == null && x.SendDate >= request.FromDate && x.SendDate <= request.ToDate)
                    .Include(x => x.Message)
                    .Include(x => x.Customer)
                    .Include(x => x.User);


            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                messages = messages.Where(x =>
                    x.User.Name.Contains(request.Search) || x.User.Family.Contains(request.Search)
                                                         || x.Message.Title.Contains(request.Search) ||
                                                         x.Message.Content.Contains(request.Search));
            }


            var userMessageList = await GetPagedAsync(request.Page, request.Limit, messages, cancellationToken);


            return userMessageList.MapTo<UserMessageDto>(_mapper);
        }
    }
}