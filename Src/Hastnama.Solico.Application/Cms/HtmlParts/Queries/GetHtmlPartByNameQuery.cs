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
    public class GetHtmlPartByNameQuery : IRequest<Result<HtmlPartDto>>
    {
        public string Name { get; set; }
    }

    public class GetHtmlPartByNameQueryHandler : IRequestHandler<GetHtmlPartByNameQuery, Result<HtmlPartDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetHtmlPartByNameQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<HtmlPartDto>> Handle(GetHtmlPartByNameQuery request,
            CancellationToken cancellationToken)
        {
            var htmlPart = await _context.HtmlParts
                .FirstOrDefaultAsync(x => x.Title == request.Name, cancellationToken);

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