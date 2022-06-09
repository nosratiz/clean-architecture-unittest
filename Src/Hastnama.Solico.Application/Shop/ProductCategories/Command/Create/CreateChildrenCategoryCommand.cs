using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Command.Create
{
    public class CreateChildrenCategoryCommand : IRequest<Result>
    {
        public int ParentId { get; set; }

        public string Name { get; set; }
    }
}