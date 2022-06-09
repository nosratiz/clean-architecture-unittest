using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Customers;
using Hastnama.Solico.Application.UserManagement.Customers.Command.SendConfirmCode;
using Hastnama.Solico.Application.UserManagement.Customers.Command.UpdateCustomer;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
  public class UpdateCustomerCommandValidatorTest
  {
      private readonly UpdateCustomerCommandValidator _validator;

      public UpdateCustomerCommandValidatorTest()
      {
          ISolicoDbContext context = new SolicoContext();
          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new UpdateCustomerCommandValidator(context,localization);
      }

      [Fact]
      public void When_Mobile_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateCustomerCommand(){Password = "R@sool1!"};

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Mobile);
      }

        [Fact]
        public void When_Password_NotSend_Must_Have_ValidationError()
        {
            var model = new UpdateCustomerCommand() { Mobile = "09126197621"};

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void When_Password_LowLength_Must_Have_ValidationError()
        {
            var model = new UpdateCustomerCommand() { Mobile = "09126197621" ,Password = "123"};

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

    }
}
