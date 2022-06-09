using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.SlideShows.Command.Delete
{
    public class DeleteSlideShowCommandHandler : IRequestHandler<DeleteSlideShowCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public DeleteSlideShowCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteSlideShowCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.Ids)
            {
                var slider = await _context.SlideShows.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (slider is null)
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.SliderNotFound, cancellationToken))));

                _context.SlideShows.Remove(slider);
            }


            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}