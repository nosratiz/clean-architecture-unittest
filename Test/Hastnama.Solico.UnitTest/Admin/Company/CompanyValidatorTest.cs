using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Companies;
using Hastnama.Solico.Application.Shop.Companies.Command.Create;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Company
{
    public class CompanyValidatorTest
    {
        private readonly CreateCompanyCommandValidator _validator;

        public CompanyValidatorTest()
        {
            ISolicoDbContext context = new SolicoContext();
            ILanguageInfo languageInfo = new LanguageInfo();
            ILocalization localization = new Localization(languageInfo);
            _validator = new CreateCompanyCommandValidator(localization,context);
        }
        
        [Fact]
        public void When_Name_NotSend_Must_Have_ValidationError()
        {
            var model = new CreateCompanyCommand();

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
        
        
        
        
    }
}