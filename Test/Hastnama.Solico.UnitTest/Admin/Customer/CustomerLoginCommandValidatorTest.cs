using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Validator.Customers;
using Hastnama.Solico.Application.UserManagement.Customers.Command.ConfirmCode;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Login;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
  public  class CustomerLoginCommandValidatorTest
  {
      private readonly CustomerLoginCommandValidator _validator;

      public CustomerLoginCommandValidatorTest()
      {
          _validator = new CustomerLoginCommandValidator();
      }

      [Fact]
      public void When_Mobile_NotSend_Must_Have_ValidationError()
      {
          var model = new CustomerLoginCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Mobile);
      }

      [Fact]
      public void When_Passwords_NotSend_Must_Have_ValidationError()
      {
          var model = new CustomerLoginCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Passwords);
      }

    }
}
