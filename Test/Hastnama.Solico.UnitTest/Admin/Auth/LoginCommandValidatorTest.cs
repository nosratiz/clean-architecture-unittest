using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Auth.Command.Login;
using Hastnama.Solico.Application.Common.Validator.Auth;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Auth
{
  public  class LoginCommandValidatorTest
  {
      private readonly LoginCommandValidator _validator;

      public LoginCommandValidatorTest()
      {
          _validator = new LoginCommandValidator();
      }

      [Fact]
      public void When_UserName_NotSend_Must_Have_ValidationError()
      {
          var model = new LoginCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x=>x.UserName);
      }

      [Fact]
      public void When_Password_NotSend_Must_Have_ValidationError()
      {
          var model = new LoginCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Password);
      }
    }
}
