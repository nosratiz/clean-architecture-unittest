using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.SlideShows.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.LanguageService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.SlideShows.Queries
{
    public class GetSliderListQuery : IRequest<List<SlidShowDto>>
    {

    }

    public class GetSliderListQueryHandler : IRequestHandler<GetSliderListQuery, List<SlidShowDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;

        public GetSliderListQueryHandler(ISolicoDbContext context, IMapper mapper, ILanguageInfo languageInfo)
        {
            _context = context;
            _mapper = mapper;
            _languageInfo = languageInfo;
        }

        public async Task<List<SlidShowDto>> Handle(GetSliderListQuery request, CancellationToken cancellationToken)
        {
            var slideShows = await _context.SlideShows
            .Where(x => x.IsVisible && (x.EndDateTime == null || x.EndDateTime > DateTime.Now))
            .ToListAsync(cancellationToken);


            return _mapper.Map<List<SlidShowDto>>(slideShows);
        }
    }
}