using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.Faqs.Command.Create;
using Hastnama.Solico.Application.Common.Validator.Faqs;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Faqs
{
  public  class CreateFaqCommandValidatorTest
  {
      private readonly CreateFaqCommandValidator _validator;

      public CreateFaqCommandValidatorTest()
      {
          _validator = new CreateFaqCommandValidator();
      }

      [Fact]
      public void When_Answer_NotSend_Must_Have_ValidationError()
      {
          var model = new CreateFaqCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Answer);
      }

      [Fact]
      public void When_Question_NotSend_Must_Have_ValidationError()
      {
          var model = new CreateFaqCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Question);
      }
    }
}
