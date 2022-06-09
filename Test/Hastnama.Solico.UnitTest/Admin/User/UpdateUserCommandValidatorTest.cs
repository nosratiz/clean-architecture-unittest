using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Users;
using Hastnama.Solico.Application.UserManagement.Users.Command.UpdateUser;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.User
{
    public class UpdateUserCommandValidatorTest
    {
        private readonly UpdateUserCommandValidator _validator;

        public UpdateUserCommandValidatorTest()
        {
            ISolicoDbContext context = new SolicoContext();
            ILanguageInfo _languageInfo = new LanguageInfo();
            ILocalization localization = new Localization(_languageInfo);
            _validator = new UpdateUserCommandValidator(context, localization);
        }

        [Fact]
        public void When_Id_NotSend_Must_Have_ValidationError()
        {
            var model = new UpdateUserCommand();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void When_Email_NotSend_Must_Have_ValidationError()
        {
            var model = new UpdateUserCommand(){Id = 1};

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void When_Email_Send_In_BadFormat_Must_Have_ValidationError()
        {
            var model = new UpdateUserCommand
            {
                Id = 1,
                Email = "rasool"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void When_Family_NotSend_Must_Have_ValidationError()
        {
            var model = new UpdateUserCommand
            {
                Id = 1,
                Email = "rasool@gmail.com"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Family);
        }

        [Fact]
        public void When_Name_NotSend_Must_Have_ValidationError()
        {
            var model = new UpdateUserCommand
            {
                Id = 1,
                Email = "rasool@gmail.com",
                Family = "Aghajani"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void When_PhoneNo_NotSend_Must_Have_ValidationError()
        {
            var model = new UpdateUserCommand()
            {
                Id = 1,
                Email = "rasool@gmail.com",
                Family = "Aghajani",
                Name = "Rasool"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void When_RoleId_NotSend_Must_Have_ValidationError()
        {
            var model = new UpdateUserCommand
            {
                Id = 1,
                Email = "rasool@gmail.com",
                Family = "Aghajani",
                Name = "Rasool",
                PhoneNumber = "09126197621"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.RoleId);
        }


     

     
    }
}
