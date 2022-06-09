using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Companies.Command.Delete
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly ILocalization _localization;

        public DeleteCompanyCommandHandler(ISolicoDbContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (company is null)
                return Result.Failed(new BadRequestObjectResult
                (new ApiMessage(await _localization.GetMessage(ResponseMessage.CompanyNotFound, cancellationToken))));

       
            company.IsDeleted = true;

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}