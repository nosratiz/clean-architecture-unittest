using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Companies.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.Companies.Queires
{
    public class GetCompanyListQuery : IRequest<List<CompanyDto>>
    {

    }

    public class GetCompanyListQueryHandler : IRequestHandler<GetCompanyListQuery, List<CompanyDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        public GetCompanyListQueryHandler(ISolicoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CompanyDto>> Handle(GetCompanyListQuery request, CancellationToken cancellationToken)
        {
            var companies = await _context.Companies.ToListAsync(cancellationToken);

            return _mapper.Map<List<CompanyDto>>(companies);
        }
    }
}