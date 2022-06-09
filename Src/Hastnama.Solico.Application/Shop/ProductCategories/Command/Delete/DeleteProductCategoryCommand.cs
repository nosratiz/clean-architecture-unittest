using System.Collections.Generic;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Command.Delete
{
    public class DeleteProductCategoryCommand : IRequest<Result>
    {
        public List<int> Ids { get; set; }
    }
}