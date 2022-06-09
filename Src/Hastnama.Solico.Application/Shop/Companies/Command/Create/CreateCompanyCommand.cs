using Hastnama.Solico.Application.Shop.Companies.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Companies.Command.Create
{
    public class CreateCompanyCommand : IRequest<Result<CompanyDto>>
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }

    }
}