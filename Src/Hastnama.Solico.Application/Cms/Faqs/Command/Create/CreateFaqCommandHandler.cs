using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.Faqs.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Faqs.Command.Create
{
    public class CreateFaqCommandHandler : IRequestHandler<CreateFaqCommand, Result<FaqDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;

        public CreateFaqCommandHandler(ISolicoDbContext context, IMapper mapper, ILanguageInfo languageInfo)
        {
            _context = context;
            _mapper = mapper;
            _languageInfo = languageInfo;
        }

        public async Task<Result<FaqDto>> Handle(CreateFaqCommand request, CancellationToken cancellationToken)
        {
            var faq = _mapper.Map<Faq>(request);
            faq.Lang = _languageInfo.LanguageCode;

            await _context.Faqs.AddAsync(faq, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<FaqDto>.SuccessFul(_mapper.Map<FaqDto>(faq));
        }
    }
}