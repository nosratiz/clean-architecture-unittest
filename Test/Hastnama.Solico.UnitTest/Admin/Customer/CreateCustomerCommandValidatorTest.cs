using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Customers;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Create;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
    public class CreateCustomerCommandValidatorTest
  {
      private readonly CreateCustomerCommandValidator _validator;

      public CreateCustomerCommandValidatorTest()
      {
          ISolicoDbContext context = new SolicoContext();
          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new CreateCustomerCommandValidator(localization,context);
      }

      [Fact]
      public void When_ID_NotSend_Must_Have_ValidationError()
      {
          var model = new CreateCustomerCommand();
         
          var result = _validator.TestValidate(model);
          
          result.ShouldHaveValidationErrorFor(x => x.SolicoCustomerId);
      }

    }
}
