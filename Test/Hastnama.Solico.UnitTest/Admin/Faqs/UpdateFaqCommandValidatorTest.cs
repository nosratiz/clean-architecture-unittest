using System;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.Faqs.Command.Update;
using Hastnama.Solico.Application.Common.Validator.Faqs;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Faqs
{
  public  class UpdateFaqCommandValidatorTest
  {
      private readonly UpdateFaqCommandValidator _validator;

      public UpdateFaqCommandValidatorTest()
      {
          _validator = new UpdateFaqCommandValidator();
      }


      [Fact]
      public void When_Id_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateFaqCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Id);
      }

      [Fact]
      public void When_Answer_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateFaqCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Answer);
      }

      [Fact]
      public void When_Question_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateFaqCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Question);
      }

    }
}
