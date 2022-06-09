using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Create;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.SlideShows;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.SlideShows
{
 public  class CreateSlideShowCommandValidatorTest
 {
     private readonly CreateSlideShowCommandValidator _validator;

     public CreateSlideShowCommandValidatorTest()
     {
         ISolicoDbContext dbContext = new SolicoContext();
         ILanguageInfo languageInfo = new LanguageInfo();
         ILocalization localization = new Localization(languageInfo);
         _validator = new CreateSlideShowCommandValidator(dbContext, localization);
     }
     [Fact]
     public void When_Image_NotSend_Must_Have_ValidationError()
     {
         var model = new CreateSlidShowCommand();

         var result = _validator.TestValidate(model);

         result.ShouldHaveValidationErrorFor(x => x.Image);
     }
     [Fact]
     public void When_Name_NotSend_Must_Have_ValidationError()
     {
         var model = new CreateSlidShowCommand(){Image = "image"};

         var result = _validator.TestValidate(model);

         result.ShouldHaveValidationErrorFor(x => x.Name);
     }

     [Fact]
     public void When_SortOrder_NotSend_Must_Have_ValidationError()
     {
         var model = new CreateSlidShowCommand(){Image = "image" ,Name = "New Name"};

         var result = _validator.TestValidate(model);

         result.ShouldHaveValidationErrorFor(x => x.SortOrder);
     }

    }
}
