using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Faqs.Command.Delete
{
    public class DeleteFaqCommandHandler : IRequestHandler<DeleteFaqCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public DeleteFaqCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteFaqCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.Ids)
            {
                var faq = await _context.Faqs.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (faq is null)
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.FaqNotFound, cancellationToken))));

                _context.Faqs.Remove(faq);
            }


            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}