using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Companies.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Companies.Command.Create
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Result<CompanyDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;

        public CreateCompanyCommandHandler(ISolicoDbContext context, IMapper mapper, ILanguageInfo languageInfo)
        {
            _context = context;
            _mapper = mapper;
            _languageInfo = languageInfo;
        }

        public async Task<Result<CompanyDto>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = _mapper.Map<Company>(request);

            company.Lang = _languageInfo.LanguageCode;

            if (string.IsNullOrWhiteSpace(request.Slug))
                company.Slug = company.Name.RemoveSpecialChar()
                .ToEnglishNumber().Trim().GenerateSeoLink();
            else
                company.Slug = request.Slug.RemoveSpecialChar()
                    .ToEnglishNumber().Trim().GenerateSeoLink();

            await _context.Companies.AddAsync(company, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<CompanyDto>.SuccessFul(_mapper.Map<CompanyDto>(company));
        }
    }
}