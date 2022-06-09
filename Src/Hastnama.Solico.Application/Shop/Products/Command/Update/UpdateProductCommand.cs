using Hastnama.Solico.Common.Result;
using MediatR;
using System.Collections.Generic;
using Hastnama.Solico.Application.Shop.Products.Dto;

namespace Hastnama.Solico.Application.Shop.Products.Command.Update
{
    public class UpdateProductCommand : IRequest<Result>
    {
        public long Id { get; set; }
        public long? ProductCategoryId { get; set; }
        
        public string MaterialType { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }

        public List<string> Tag { get; set; }

        public string Unit { get; set; }

        public List<string> Galleries { get; set; }
        
        public bool IsSpecial { get; set; }
        
        public List<UpdateProductMediaDto> UpdateProductMedia { get; set; }
    }
}