using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.ContactUses.Command.Delete
{
    public class DeleteContactUsCommandHandler : IRequestHandler<DeleteContactUsCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public DeleteContactUsCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteContactUsCommand request, CancellationToken cancellationToken)
        {

            foreach (var id in request.Ids)
            {
                var contactUs = await _context.ContactUs.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (contactUs is null)
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ContentNotFound, cancellationToken))));

                _context.ContactUs.Remove(contactUs);
            }

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}