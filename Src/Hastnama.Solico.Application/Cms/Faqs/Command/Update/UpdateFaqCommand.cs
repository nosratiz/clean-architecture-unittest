using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Faqs.Command.Update
{
    public class UpdateFaqCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
    }
}