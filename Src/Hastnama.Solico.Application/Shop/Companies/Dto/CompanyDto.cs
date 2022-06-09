using System;

namespace Hastnama.Solico.Application.Shop.Companies.Dto
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public DateTime CreateDate { get; set; }
    }
}