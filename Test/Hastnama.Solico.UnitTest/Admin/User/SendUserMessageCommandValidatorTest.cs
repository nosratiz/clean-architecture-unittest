using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.Messages.Command.SendCustomerMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.SendUserMessage;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Messages;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.User
{
   public class SendUserMessageCommandValidatorTest
   {
       private readonly SendUserMessageCommandValidator _validator;

       public SendUserMessageCommandValidatorTest()
       {
           _validator = new SendUserMessageCommandValidator();
       }

       [Fact]
       public void When_Content_NotSend_Must_Have_ValidationError()
       {
           var model = new SendUserMessageCommand();

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.Content);
       }

       [Fact]
       public void When_Title_NotSend_Must_Have_ValidationError()
       {
           var model = new SendUserMessageCommand();

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.Title);
       }
    }
}
