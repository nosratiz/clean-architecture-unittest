namespace Hastnama.Solico.Domain.Models.Shop
{
    public class ProductGallery
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Image { get; set; }

        public virtual Product Product { get; set; }
    }
}