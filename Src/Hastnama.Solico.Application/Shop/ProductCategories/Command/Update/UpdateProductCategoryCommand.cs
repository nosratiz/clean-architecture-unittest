using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Command.Update
{
    public class UpdateProductCategoryCommand : IRequest<Result>
    {

        public int Id { get; set; }
        public int? ParentId { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public bool IsActive { get; set; }
    }
}