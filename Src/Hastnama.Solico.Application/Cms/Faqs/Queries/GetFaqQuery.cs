using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Faqs.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Faqs.Queries
{
    public class GetFaqQuery : IRequest<Result<FaqDto>>
    {
        public int Id { get; set; }
    }

    public class GetFaqQueryHandler : IRequestHandler<GetFaqQuery, Result<FaqDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetFaqQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<FaqDto>> Handle(GetFaqQuery request, CancellationToken cancellationToken)
        {
            var faq = await _context.Faqs.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (faq is null)
                return Result<FaqDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.FaqNotFound, cancellationToken))));


            return Result<FaqDto>.SuccessFul(_mapper.Map<FaqDto>(faq));
        }
    }
}