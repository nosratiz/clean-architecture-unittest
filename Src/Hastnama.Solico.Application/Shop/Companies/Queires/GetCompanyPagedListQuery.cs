using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.Companies.Dto;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Companies.Queires
{
    public class GetCompanyPagedListQuery : PagingOptions, IRequest<PagedList<CompanyDto>>
    {

    }

    public class GetCompanyPagedListQueryHandler : PagingService<Company>, IRequestHandler<GetCompanyPagedListQuery, PagedList<CompanyDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;


        public GetCompanyPagedListQueryHandler(ISolicoDbContext context, IMapper mapper, ILanguageInfo languageInfo)
        {
            _context = context;
            _mapper = mapper;
            _languageInfo = languageInfo;
        }

        public async Task<PagedList<CompanyDto>> Handle(GetCompanyPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Company> companies = _context.Companies.Where(x => x.Lang == _languageInfo.LanguageCode);

            if (!string.IsNullOrWhiteSpace(request.Search))
                companies = companies.Where(x => x.Name.Contains(request.Search));


            var CompanyPagedList = await GetPagedAsync(request.Page, request.Limit, companies,cancellationToken);

            return CompanyPagedList.MapTo<CompanyDto>(_mapper);
        }
    }
}