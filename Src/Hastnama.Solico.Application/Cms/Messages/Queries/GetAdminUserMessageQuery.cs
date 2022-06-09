using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Messages.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper.Claims.User;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Messages.Queries
{
    public class GetAdminUserMessageQuery : PagingOptions, IRequest<PagedList<UserMessageDto>>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    public class GetAdminUserMessageQueryHandler : PagingService<UserMessage>,
        IRequestHandler<GetAdminUserMessageQuery, PagedList<UserMessageDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAdminUserMessageQueryHandler(ISolicoDbContext context, IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagedList<UserMessageDto>> Handle(GetAdminUserMessageQuery request,
            CancellationToken cancellationToken)
        {
            request.FromDate ??= new DateTime();
            request.ToDate ??= DateTime.Now;

            IQueryable<UserMessage> messages =
                _context.UserMessages
                    .OrderByDescending(x => x.SendDate)
                    .Where(x => x.UserId == long.Parse(_currentUserService.UserId)
                                && x.UserHasDeleted == false &&
                                x.Message.ParentId == null && x.SendDate >= request.FromDate &&
                                x.SendDate <= request.ToDate)
                    .Include(x => x.Message)
                    .Include(x => x.Customer)
                    .Include(x => x.User);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                messages = messages.Where(x =>
                    x.Customer.FullName.Contains(request.Search)
                    || x.Message.Title.Contains(request.Search) ||
                    x.Message.Content.Contains(request.Search));
            }


            var userMessageList = await GetPagedAsync(request.Page, request.Limit, messages, cancellationToken);


            return userMessageList.MapTo<UserMessageDto>(_mapper);
        }
    }
}