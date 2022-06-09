using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Cms.HtmlParts.Dto;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Cms;
using MediatR;

namespace Hastnama.Solico.Application.Cms.HtmlParts.Command.Create
{
    public class CreateHtmlPartCommandHandler : IRequestHandler<CreateHtmlPartCommand, Result<HtmlPartDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public CreateHtmlPartCommandHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<HtmlPartDto>> Handle(CreateHtmlPartCommand request,
            CancellationToken cancellationToken)
        {
            var htmlPart = _mapper.Map<HtmlPart>(request);

            await _context.HtmlParts.AddAsync(htmlPart, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<HtmlPartDto>.SuccessFul(_mapper.Map<HtmlPartDto>(htmlPart));
        }
    }
}