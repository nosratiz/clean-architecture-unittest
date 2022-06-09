using System.Collections.Generic;

namespace Hastnama.Solico.Application.Shop.ProductCategories.Dto
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int ProductCount { get; set; }


        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public bool IsActive { get; set; }

        public List<ProductCategoryDto> Children { get; set; }


    }


    public class AdminProductCategoryDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int ProductCount { get; set; }


        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public bool IsActive { get; set; }
        
        public List<ProductCategoryDto> Children { get; set; }
    }
}