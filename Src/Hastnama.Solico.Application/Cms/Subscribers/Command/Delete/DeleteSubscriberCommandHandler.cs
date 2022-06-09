using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.Subscribers.Command.Delete
{
    public class DeleteSubscriberCommandHandler : IRequestHandler<DeleteSubscriberCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;


        public DeleteSubscriberCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteSubscriberCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.Ids)
            {
                var subscriber = await _context.Subscribes.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (subscriber is null)
                {
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage(
                        await _localization.GetMessage(ResponseMessage.SubscriberNotFound, cancellationToken))));
                }

                _context.Subscribes.Remove(subscriber);
            }

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}