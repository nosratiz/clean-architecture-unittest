using System;

namespace Hastnama.Solico.Application.Shop.Products.Dto
{
    public class ProductMediaDto
    {
        public long Id { get; set; }
        
        public string Icon { get; set; }

        public string Description { get; set; }
    }

    public class UpdateProductMediaDto
    {
        public string Icon { get; set; }

        public string Description { get; set; }
    }
}