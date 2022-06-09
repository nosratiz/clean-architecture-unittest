using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Companies.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Companies.Queires
{
    public class GetCompanyQuery : IRequest<Result<CompanyDto>>
    {
        public int Id { get; set; }
    }

    public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, Result<CompanyDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetCompanyQueryHandler(ISolicoDbContext context, ILocalization localization, IMapper mapper)
        {
            _context = context;
            _localization = localization;
            _mapper = mapper;
        }

        public async Task<Result<CompanyDto>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (company is null)
                return Result<CompanyDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                        await _localization.GetMessage(ResponseMessage.CompanyNotFound, cancellationToken))));


            return Result<CompanyDto>.SuccessFul(_mapper.Map<CompanyDto>(company));
        }
    }
}