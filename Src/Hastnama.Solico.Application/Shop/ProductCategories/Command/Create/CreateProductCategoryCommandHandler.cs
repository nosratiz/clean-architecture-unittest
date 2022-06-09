using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Shop.ProductCategories.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Command.Create
{
    public class
        CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, Result<ProductCategoryDto>>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILanguageInfo _languageInfo;
        private readonly ILocalization _localization;

        public CreateProductCategoryCommandHandler(IMapper mapper, ISolicoDbContext context, ILanguageInfo languageInfo,
            ILocalization localization)
        {
            _mapper = mapper;
            _context = context;
            _languageInfo = languageInfo;
            _localization = localization;
        }

        public async Task<Result<ProductCategoryDto>> Handle(CreateProductCategoryCommand request,
            CancellationToken cancellationToken)
        {
            if (request.ChildrenNames.Any(x => x == request.Name))
            {
                return Result<ProductCategoryDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.DuplicateName, cancellationToken))));
            }

            var category = _mapper.Map<ProductCategory>(request);

            category.Lang = _languageInfo.LanguageCode;

            if (string.IsNullOrWhiteSpace(category.Slug))
                category.Slug = category.Name.RemoveSpecialChar().ToEnglishNumber()
                    .GenerateSeoLink()
                    .TrimEnd();

            await _context.ProductCategories.AddAsync(category, cancellationToken);

            await AddChildrenAsync(request, category, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            return Result<ProductCategoryDto>.SuccessFul(_mapper.Map<ProductCategoryDto>(category));
        }

        private async Task AddChildrenAsync(CreateProductCategoryCommand request, ProductCategory category,
            CancellationToken cancellationToken)
        {
            foreach (var children in request.ChildrenNames.Distinct())
            {
                await _context.ProductCategories.AddAsync(new ProductCategory
                {
                    Name = children,
                    Lang = _languageInfo.LanguageCode,
                    Parent = category,
                    Slug = children
                        .RemoveSpecialChar()
                        .ToEnglishNumber()
                        .Trim()
                        .GenerateSeoLink()
                }, cancellationToken);
            }
        }
    }
}