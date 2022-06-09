using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.Messages.Command.SendCustomerMessage;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Messages;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
  public  class SendCustomerMessageValidatorTest
  {
      private readonly SendCustomerMessageValidator _validator;

      public SendCustomerMessageValidatorTest()
      {
          ISolicoDbContext dbContext = new SolicoContext();
          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new SendCustomerMessageValidator(dbContext, localization);
      }

      [Fact]
      public void When_Content_NotSend_Must_Have_ValidationError()
      {
          var model = new SendCustomerMessageCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Content);
      }


      [Fact]
      public void When_Title_NotSend_Must_Have_ValidationError()
      {
          var model = new SendCustomerMessageCommand(){Content = "Content"};

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Title);
      }

    }
}
