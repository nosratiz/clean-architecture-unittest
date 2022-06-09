using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Companies.Command.Update
{
    public class UpdateCompanyCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    }
}