using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.HtmlParts.Command.Update
{
    public class UpdateHtmlPartCommandHandler : IRequestHandler<UpdateHtmlPartCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public UpdateHtmlPartCommandHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateHtmlPartCommand request, CancellationToken cancellationToken)
        {
            var htmlPart = await _context.HtmlParts.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (htmlPart is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.HtmlPartNotFound,
                        cancellationToken))));
            }

            _mapper.Map(request, htmlPart);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}