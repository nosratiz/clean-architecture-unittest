using System;
using System.Collections.Generic;
using Hastnama.Solico.Domain.Models.Statistic;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class Product
    {
        public Product()
        {
            ProductGalleries = new HashSet<ProductGallery>();
            OrderItems = new HashSet<OrderItem>();
            CustomerProductViews = new HashSet<CustomerProductView>();
            CustomerProductPrices = new HashSet<CustomerProductPrice>();
            ProductMedias = new HashSet<ProductMedia>();
        }

        public long Id { get; set; }
        public long? ProductCategoryId { get; set; }

        public string Image { get; set; }
        public string MaterialId { get; set; }
        public string MaterialType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string Tag { get; set; }
        public string Unit { get; set; }

        public string Division { get; set; }

        public string Lang { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsSpecial { get; set; }
        public bool IsDeleted { get; set; }


        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<ProductGallery> ProductGalleries { get; }
        public virtual ICollection<OrderItem> OrderItems { get; }
        public virtual ICollection<CustomerProductView> CustomerProductViews { get; }
        public virtual ICollection<CustomerProductPrice> CustomerProductPrices { get; }
        public virtual ICollection<ProductMedia> ProductMedias { get; }
    }
}