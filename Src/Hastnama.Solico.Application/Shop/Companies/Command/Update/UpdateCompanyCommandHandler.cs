using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Companies.Command.Update
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public UpdateCompanyCommandHandler(IMapper mapper, ISolicoDbContext context, ILocalization localization)
        {
            _mapper = mapper;
            _context = context;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await GetCompanyAsync(request, cancellationToken);

            if (company is null)
                return Result.Failed(new BadRequestObjectResult
                (new ApiMessage(await _localization.GetMessage(ResponseMessage.CompanyNotFound, cancellationToken))));

            _mapper.Map(request, company);

            company.Slug = request.Slug
            .RemoveSpecialChar()
            .ToEnglishNumber().Trim()
            .GenerateSeoLink();

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();

        }

        #region Query

        private async Task<Company> GetCompanyAsync(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            return await _context.Companies
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        }

        #endregion
       
    }
}