using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.ProductCategories;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Create;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.ProductCategory
{
  public  class CreateProductCategoryCommandValidatorTest
  {
      private readonly CreateProductCategoryCommandValidator _validator;

      public CreateProductCategoryCommandValidatorTest()
      {
          ISolicoDbContext dbContext = new SolicoContext();
          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new CreateProductCategoryCommandValidator(localization,dbContext);
      }

      [Fact]
      public void When_Name_NotSend_Must_Have_ValidationError()
      {
          var model = new CreateProductCategoryCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Name);
      }


    }
}
