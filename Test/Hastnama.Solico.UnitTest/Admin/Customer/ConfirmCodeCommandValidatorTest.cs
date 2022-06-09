using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Validator.Customers;
using Hastnama.Solico.Application.UserManagement.Customers.Command.ConfirmCode;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
   public class ConfirmCodeCommandValidatorTest
    {
        private readonly ConfirmCodeCommandValidator _validator;

        public ConfirmCodeCommandValidatorTest()
        {
            _validator = new ConfirmCodeCommandValidator();
        }
        [Fact]
        public void When_Code_NotSend_Must_Have_ValidationError()
        {
            var model = new ConfirmCodeCommand();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Code);
        }

        [Fact]
        public void When_Mobile_NotSend_Must_Have_ValidationError()
        {
            var model = new ConfirmCodeCommand();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Mobile);
        }
    }
}
