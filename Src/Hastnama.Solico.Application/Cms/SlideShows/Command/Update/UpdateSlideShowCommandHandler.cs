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

namespace Hastnama.Solico.Application.Cms.SlideShows.Command.Update
{
    public class UpdateSlideShowCommandHandler : IRequestHandler<UpdateSlideShowCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public UpdateSlideShowCommandHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateSlideShowCommand request, CancellationToken cancellationToken)
        {
            var slider = await _context.SlideShows.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (slider is null)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.SliderNotFound, cancellationToken))));

            _mapper.Map(request, slider);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();

        }
    }
}