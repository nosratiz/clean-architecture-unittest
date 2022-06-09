using System;

namespace Hastnama.Solico.Domain.Models.Shop
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Lang { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}