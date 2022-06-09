using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Validator.Customers;
using Hastnama.Solico.Application.UserManagement.Customers.Command.SendConfirmCode;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
  public  class SendConfirmCodeCommandValidatorTest
  {
      private readonly SendConfirmCodeCommandValidator _validator;

      public  SendConfirmCodeCommandValidatorTest()
      {
          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new SendConfirmCodeCommandValidator(localization);
      }

      [Fact]
      public void When_Mobile_NotSend_Must_Have_ValidationError()
      {
          var model = new SendConfirmCodeCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Mobile);
      }
    }
}
