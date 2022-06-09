using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.Shop;
using MediatR;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Command.Create
{
    public class CreateChildrenCategoryCommandHandler : IRequestHandler<CreateChildrenCategoryCommand,Result>
    {
        private readonly ISolicoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;
        private readonly ILanguageInfo _languageInfo;

        public CreateChildrenCategoryCommandHandler(ISolicoDbContext context, IMapper mapper, ILocalization localization, ILanguageInfo languageInfo)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
            _languageInfo = languageInfo;
        }

        public async Task<Result> Handle(CreateChildrenCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<ProductCategory>(request);

            category.Lang = _languageInfo.LanguageCode;

            if (string.IsNullOrWhiteSpace(category.Slug))
                category.Slug = category.Name.RemoveSpecialChar().ToEnglishNumber()
                    .GenerateSeoLink()
                    .TrimEnd();
            
            await _context.ProductCategories.AddAsync(category, cancellationToken);

            await _context.SaveAsync(cancellationToken);
            
            return Result.SuccessFul();
        }
    }
}