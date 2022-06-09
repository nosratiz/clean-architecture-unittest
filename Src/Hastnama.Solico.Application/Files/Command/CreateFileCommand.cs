using Hastnama.Solico.Application.Files.Dto;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hastnama.Solico.Application.Files.Command
{
    public class CreateFileCommand : IRequest<Result<FileDto>>
    {
        public IFormFile Files { get; set; }
        public string Type { get; set; }
    }
}