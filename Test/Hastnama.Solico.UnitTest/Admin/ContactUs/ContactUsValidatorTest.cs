using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Cms.ContactUses.Command.Create;
using Hastnama.Solico.Application.Common.Validator.ContactUses;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.ContactUs
{
    public class ContactUsValidatorTest
    {
        private readonly CreateContactUsCommandValidator _validator;

        public ContactUsValidatorTest()
        {
            ILanguageInfo languageInfo = new LanguageInfo();
            ILocalization localization = new Localization(languageInfo);
            _validator = new CreateContactUsCommandValidator(localization);
        }

        [Fact]
        public void When_Name_NotSend_Must_Have_ValidationError()
        {
            var model = new CreateContactUsCommand();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void When_Email_NotSend_Must_Have_ValidationError()
        {
            var model = new CreateContactUsCommand(){Email = "",Message = "Message",Name = "Rasool"};

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        [Fact]
        public void When_Email_NotValid_Must_Have_ValidationError()
        {
            var model = new CreateContactUsCommand() { Email = "Rasool.com", Message = "Message", Name = "Rasool" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void When_Message_NotSend_Must_Have_ValidationError()
        {
            var model = new CreateContactUsCommand() { Email = "Aghajani.Rasool@Gmail.com", Message = "", Name = "Rasool" };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Message);
        }

    }
}
