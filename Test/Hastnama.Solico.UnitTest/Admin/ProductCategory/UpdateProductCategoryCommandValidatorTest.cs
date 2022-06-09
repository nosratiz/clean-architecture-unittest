using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Validator.ProductCategories;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Create;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Update;
using Hastnama.Solico.Common.LanguageService;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Persistence.Context;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.ProductCategory
{
   public class UpdateProductCategoryCommandValidatorTest
   {
       private readonly UpdateProductCategoryCommandValidator _validator;

       public UpdateProductCategoryCommandValidatorTest()
       {
           ISolicoDbContext dbContext = new SolicoContext();
            ILanguageInfo languageInfo = new LanguageInfo();
           ILocalization localization = new Localization(languageInfo);
           _validator = new UpdateProductCategoryCommandValidator(localization, dbContext);
       }

       [Fact]
       public void When_Id_NotSend_Must_Have_ValidationError()
       {
           var model = new UpdateProductCategoryCommand();

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.Id);
       }

       [Fact]
       public void When_Name_NotSend_Must_Have_ValidationError()
       {
           var model = new UpdateProductCategoryCommand(){ Id = 1};

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.Name);
       }

       [Fact]
       public void When_Slug_NotSend_Must_Have_ValidationError()
       {
           var model = new UpdateProductCategoryCommand(){Id = 1 , Name = "Rasool"};

           var result = _validator.TestValidate(model);

           result.ShouldHaveValidationErrorFor(x => x.Slug);
       }
    }
}
