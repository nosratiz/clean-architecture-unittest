using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.HtmlParts.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.HtmlParts.Queries
{
    public class GetHtmlPartQuery : IRequest<Result<HtmlPartDto>>
    {
        public long Id { get; set; }
    }

    public class GetHtmlPartQueryHandler : IRequestHandler<GetHtmlPartQuery, Result<HtmlPartDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetHtmlPartQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<HtmlPartDto>> Handle(GetHtmlPartQuery request, CancellationToken cancellationToken)
        {
            var htmlPart = await _context.HtmlParts.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (htmlPart is null)
            {
                return Result<HtmlPartDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.HtmlPartNotFound,
                        cancellationToken))));
            }

            return Result<HtmlPartDto>.SuccessFul(_mapper.Map<HtmlPartDto>(htmlPart));
        }
    }
}