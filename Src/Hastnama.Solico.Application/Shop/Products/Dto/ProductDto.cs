using System;
using System.Collections.Generic;
using Hastnama.Solico.Application.Shop.ProductCategories.Dto;

namespace Hastnama.Solico.Application.Shop.Products.Dto
{
    public class 
        ProductDto
    {
        public long Id { get; set; }
        public long? ProductCategoryId { get; set; }
        public string CategoryName { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }
        public string MaterialId { get; set; }
        public string MaterialType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string Division { get; set; }
        public List<string> Tag { get; set; }
        public List<string> Galleries { get; set; }
        public string Unit { get; set; }


        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsSpecial { get; set; }
        
        public List<ProductMediaDto> ProductMedias { get; set; }
        
        public ProductCategoryDto ProductCategory { get; set; }
    }
}