using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Cms.CustomerConsults.Command.Delete
{
    public class DeleteCustomerConsultCommandHandler : IRequestHandler<DeleteCustomerConsultCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public DeleteCustomerConsultCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteCustomerConsultCommand request, CancellationToken cancellationToken)
        {
            var consult =
                await _context.CustomerConsults.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            if (consult is null)
            {
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ContentNotFound,
                        cancellationToken))));
            }

            consult.IsDelete = true;

            await _context.SaveAsync(cancellationToken);
            
            return Result.SuccessFul();
        }
    }
}