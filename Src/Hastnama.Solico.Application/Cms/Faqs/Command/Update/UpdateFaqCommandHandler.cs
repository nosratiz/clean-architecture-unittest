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

namespace Hastnama.Solico.Application.Cms.Faqs.Command.Update
{
    public class UpdateFaqCommandHandler : IRequestHandler<UpdateFaqCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public UpdateFaqCommandHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
        {
            var faq = await _context.Faqs.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (faq is null)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.FaqNotFound, cancellationToken))));

            _mapper.Map(request, faq);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();

        }
    }
}