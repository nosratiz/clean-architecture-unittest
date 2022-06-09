using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Validator.File;
using Hastnama.Solico.Application.Files.Command;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Options;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Common.Files
{
  public  class CreateFileCommandValidatorTest
  {
      private readonly CreateFileCommandValidator _validator;
      
      public CreateFileCommandValidatorTest()
      {
          IOptionsMonitor<FileExtensions> fileExtensions = new Mock<IOptionsMonitor<FileExtensions>>().Object;

          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new CreateFileCommandValidator(fileExtensions, localization);
      }

      [Fact]
      public void When_Files_NotSend_Must_Have_ValidationError()
      {
          var model = new CreateFileCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveAnyValidationError();
      }

  
    }
}
