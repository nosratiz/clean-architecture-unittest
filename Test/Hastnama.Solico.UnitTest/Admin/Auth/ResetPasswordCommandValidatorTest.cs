using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Auth.Command.ResetPassword;
using Hastnama.Solico.Application.Common.Validator.Auth;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Auth
{
   public class ResetPasswordCommandValidatorTest
   {
       private readonly ResetPasswordCommandValidator _validator;

       public ResetPasswordCommandValidatorTest()
       {
           _validator = new ResetPasswordCommandValidator();
       }

       [Fact]
       public void When_ActiveCode_NotSend_Must_Have_ValidationError()
       {
           var model = new ResetPasswordCommand();

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.ActiveCode);
       }

       [Fact]
       public void When_Email_NotSend_Must_Have_ValidationError()
       {
           var model = new ResetPasswordCommand();

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.Email);
       }

       [Fact]
       public void When_NewPassword_NotSend_Must_Have_ValidationError()
       {
           var model = new ResetPasswordCommand();

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.NewPassword);
       }
    }
}
