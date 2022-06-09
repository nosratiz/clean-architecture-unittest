using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Users;
using Hastnama.Solico.Application.UserManagement.Users.Command.CreateUser;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.User
{
    public class CreateUserCommandValidatorTest
    {
        private readonly CreateUserCommandValidator _validator;

        public CreateUserCommandValidatorTest()
        {
            ISolicoDbContext context = new SolicoContext();
            ILanguageInfo _languageInfo = new LanguageInfo();
            ILocalization localization = new Localization(_languageInfo);
            _validator = new CreateUserCommandValidator(context,localization);
        }
        
        
        [Fact]
        public void When_Email_NotSend_Must_Have_ValidationError()
        {
            var model = new CreateUserCommand();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void When_Email_Send_In_BadFormat_Must_Have_ValidationError()
        {
            var model = new CreateUserCommand
            {
                Email = "nima"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
        
        [Fact]
        public void When_Family_NotSend_Must_Have_ValidationError()
        {
            var model = new CreateUserCommand
            {
                Email = "nima@gmail.com"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Family);
        }
        
        [Fact]
        public void When_Name_NotSend_Must_Have_ValidationError()
        {
            var model = new CreateUserCommand
            {
                Email = "nima@gmail.com",
                Family = "Nosrati"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        
        [Fact]
        public void When_Password_NotSend_Must_Have_ValidationError()
        {
            var model = new CreateUserCommand
            {
                Email = "nima@gmail.com",
                Family = "Nosrati",
                Name = "Nima"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }


       

        [Theory]
        [InlineData("9126197621")]
        public void When_PhoneNumberExist_Must_Have_ValidationError(string phoneNo)
        {
            var model = new CreateUserCommand {Email  = "nima@gmail.com" ,Name = "nima" ,Family = "nosrati", Password = "1234", RoleId = 1,PhoneNumber = phoneNo };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }


    }
}