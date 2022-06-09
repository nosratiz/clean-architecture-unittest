using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Shop.Companies.Command.Delete
{
    public class DeleteCompanyCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}