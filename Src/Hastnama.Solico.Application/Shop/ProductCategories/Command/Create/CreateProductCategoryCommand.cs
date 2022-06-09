using System.Collections.Generic;
using Hastnama.Solico.Application.Shop.ProductCategories.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Command.Create
{
    public class CreateProductCategoryCommand : IRequest<Result<ProductCategoryDto>>
    {
        public string Name { get; set; }

        public List<string> ChildrenNames { get; set; }
    }
}