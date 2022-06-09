using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.Messages.Command.ReplyCustomerMessage;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Messages;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
  public class ReplyCustomerMessageCommandValidatorTest
  {
      private readonly ReplyCustomerCommandValidator _validator;

     public ReplyCustomerMessageCommandValidatorTest()
      {
            ISolicoDbContext dbContext = new SolicoContext();
            ILanguageInfo languageInfo = new LanguageInfo();
            ILocalization localization = new Localization(languageInfo);
            _validator = new ReplyCustomerCommandValidator(dbContext, localization);
        }
     [Fact]
     public void When_Content_NotSend_Must_Have_ValidationError()
     {
         var model = new ReplyCustomerMessageCommand();

         var result = _validator.TestValidate(model);

         result.ShouldHaveValidationErrorFor(x => x.Content);
     }
    }
}
