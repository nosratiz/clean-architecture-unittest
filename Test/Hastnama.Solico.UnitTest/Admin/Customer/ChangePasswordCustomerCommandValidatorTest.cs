using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.ContactUses.Command.Create;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Customers;
using Hastnama.Solico.Application.UserManagement.Customers.Command.ChangePassword;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
  public class ChangePasswordCustomerCommandValidatorTest
    {
        private readonly ChangePasswordCustomerCommandValidator _validator;

        public ChangePasswordCustomerCommandValidatorTest()
        {
            ILanguageInfo languageInfo = new LanguageInfo();
            ILocalization localization = new Localization(languageInfo);
            _validator = new ChangePasswordCustomerCommandValidator(localization);
        }
          [Fact]
        public void When_Password_NotSend_Must_Have_ValidationError()
        {
            var model = new ChangePasswordCustomerCommand();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void When_NewPassword_NotSend_Must_Have_ValidationError()
        {
            var model = new ChangePasswordCustomerCommand(){Password = "123"};

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }
    }
}
