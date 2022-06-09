using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Update;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.SlideShows;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.SlideShows
{
  public  class UpdateSlidShowCommandValidatorTest
  {
      private readonly UpdateSlidShowCommandValidator _validator;

      public UpdateSlidShowCommandValidatorTest()
      {
          ISolicoDbContext dbContext = new SolicoContext();
          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new UpdateSlidShowCommandValidator(localization,dbContext);
      }

      [Fact]
      public void When_Image_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateSlideShowCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Image);
      }

      [Fact]
      public void When_Name_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateSlideShowCommand(){Image = "UpdateImage"};

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Name);
      }

      [Fact]
      public void When_SortOrder_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateSlideShowCommand() { Image = "UpdateImage" , Name = "UpdateName"};

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.SortOrder);
      }

    }
}
