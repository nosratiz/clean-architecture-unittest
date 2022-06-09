using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.Products;
using Hastnama.Solico.Application.Shop.Products.Command.Update;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Product
{
  public  class UpdateProductCommandValidatorTest
  {
      private readonly UpdateProductCommandValidator _validator;

      public UpdateProductCommandValidatorTest()
      {
          ISolicoDbContext dbContext = new SolicoContext();
          ILanguageInfo languageInfo = new LanguageInfo();
          ILocalization localization = new Localization(languageInfo);
          _validator = new UpdateProductCommandValidator(dbContext , localization);
      }

      [Fact]
      public void When_Name_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateProductCommand();

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.Name);
      }

      [Fact]
      public void When_Id_NotSend_Must_Have_ValidationError()
      {
          var model = new UpdateProductCommand(){Name = "New Name Product"};

          var result = _validator.TestValidate(model);

          result.ShouldHaveValidationErrorFor(x => x.ProductCategoryId);
      }
    }
}
