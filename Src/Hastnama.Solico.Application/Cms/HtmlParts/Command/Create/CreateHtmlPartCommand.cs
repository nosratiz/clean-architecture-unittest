using Hastnama.Solico.Application.Cms.HtmlParts.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;

namespace Hastnama.Solico.Application.Cms.HtmlParts.Command.Create
{
    public class CreateHtmlPartCommand : IRequest<Result<HtmlPartDto>>
    {
        public string Name { get; set; }

        public string Content { get; set; }
    }
}