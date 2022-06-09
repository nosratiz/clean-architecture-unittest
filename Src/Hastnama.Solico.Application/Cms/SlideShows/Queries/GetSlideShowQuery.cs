using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.SlideShows.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.SlideShows.Queries
{
    public class GetSlideShowQuery : IRequest<Result<SlidShowDto>>
    {
        public int Id { get; set; }
    }
    
    public class GetSlideShowQueryHandler : IRequestHandler<GetSlideShowQuery,Result<SlidShowDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetSlideShowQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<SlidShowDto>> Handle(GetSlideShowQuery request, CancellationToken cancellationToken)
        {
            var slider = await _context.SlideShows.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (slider is null)
                return Result<SlidShowDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.SliderNotFound, cancellationToken))));
            
            return Result<SlidShowDto>.SuccessFul(_mapper.Map<SlidShowDto>(slider));

        }
    }
}