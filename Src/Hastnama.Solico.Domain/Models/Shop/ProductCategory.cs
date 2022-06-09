using System.Collections.Generic;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class ProductCategory
    {
        public  ProductCategory()
        {
            Children = new HashSet<ProductCategory>();
            Products = new HashSet<Product>();
        }

        public long Id { get; set; }
        public long? ParentId { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public string Lang { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


        public virtual ProductCategory Parent { get; set; }
        public virtual ICollection<ProductCategory> Children { get; }
        public virtual ICollection<Product> Products { get; }
        
        
    }
}