using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Update;
using Hastnama.Solico.Application.Cms.Subscribers.Command.Create;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.SlideShows;
using Hastnama.Solico.Application.Common.Validator.Subscribers;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Subscribers
{
   public class CreateSubscriberCommandValidatorTest
   {
       private readonly CreateSubscriberCommandValidator _validator;

       public CreateSubscriberCommandValidatorTest()
       {
           ISolicoDbContext dbContext = new SolicoContext();
           ILanguageInfo languageInfo = new LanguageInfo();
           ILocalization localization = new Localization(languageInfo);
           _validator = new CreateSubscriberCommandValidator(localization, dbContext);
       }
       [Fact]
       public void When_Email_NotSend_Must_Have_ValidationError()
       {
           var model = new CreateSubscriberCommand();

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.Email);
       }
    }
}
