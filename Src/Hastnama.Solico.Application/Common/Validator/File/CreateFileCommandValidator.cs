using System.IO;
using FluentValidation;
using Hastnama.Solico.Application.Files.Command;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Options;
using Microsoft.Extensions.Options;

namespace Hastnama.Solico.Application.Common.Validator.File
{
    public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
    {
        private readonly IOptionsMonitor<FileExtensions> _fileExtensions;
        private readonly ILocalization _localization;
        

        public CreateFileCommandValidator(IOptionsMonitor<FileExtensions> fileExtensions, ILocalization localization)
        {
            _fileExtensions = fileExtensions;
            _localization = localization;

            
            RuleFor(dto => dto)
                .Must(ValidExtension)
                .WithMessage(_localization.GetMessage(ResponseMessage.FileExtensionNotSupported));

        }

        private bool ValidExtension(CreateFileCommand uploadFileCommand)
        {

            if (uploadFileCommand.Files == null || uploadFileCommand.Files.Length == 0)
                return false;

            var fileExtension = Path.GetExtension(uploadFileCommand.Files.FileName);

            if (!_fileExtensions.CurrentValue.ValidFormat.Exists(x => x == fileExtension))
                return false;


            return true;
        }
    }
}