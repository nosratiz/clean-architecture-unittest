using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.SlideShows.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.SlideShows.Command.Create
{
    public class CreateSlidShowCommandHandler : IRequestHandler<CreateSlidShowCommand, Result<SlidShowDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;

        public CreateSlidShowCommandHandler(ISolicoDbContext context, IMapper mapper, ILanguageInfo languageInfo)
        {
            _context = context;
            _mapper = mapper;
            _languageInfo = languageInfo;
        }

        public async Task<Result<SlidShowDto>> Handle(CreateSlidShowCommand request, CancellationToken cancellationToken)
        {
            var slideShow = _mapper.Map<SlideShow>(request);

            slideShow.Lang = _languageInfo.LanguageCode;

            await _context.SlideShows.AddAsync(slideShow, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<SlidShowDto>.SuccessFul(_mapper.Map<SlidShowDto>(slideShow));
        }
    }
}