using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.Messages.Command.ReplyUserMessage;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Messages;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.User
{
  public  class ReplyUserMessageCommandValidatorTest
  {
      private readonly ReplyUserMessageCommandValidator _validator;

      public ReplyUserMessageCommandValidatorTest()
      {
          ISolicoDbContext dbContext = new SolicoContext();
          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new ReplyUserMessageCommandValidator(dbContext, localization);
      }

      [Fact]
      public void When_Content_NotSend_Must_Have_ValidationError()
      {
          var model = new ReplyUserMessageCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Content);
      }
    }
}
