using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.ProductCategories.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Queries
{
    public class GetProductCategoryQuery : IRequest<Result<ProductCategoryDto>>
    {
        public int Id { get; set; }
    }

    public class GetProductCategoryQueryHandler : IRequestHandler<GetProductCategoryQuery, Result<ProductCategoryDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;

        private readonly ILocalization _localization;

        public GetProductCategoryQueryHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<ProductCategoryDto>> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.ProductCategories
            .Include(x => x.Children)
            .ThenInclude(x => x.Children)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category is null)
                return Result<ProductCategoryDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.CategoryNotFound, cancellationToken))));

            return Result<ProductCategoryDto>.SuccessFul(_mapper.Map<ProductCategoryDto>(category));
        }
    }
}