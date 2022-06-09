using Hastnama.Solico.Application.Cms.Faqs.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.Faqs.Command.Create
{
    public class CreateFaqCommand : IRequest<Result<FaqDto>>
    {
        public string Answer { get; set; }
        public string Question { get; set; }
    }
}