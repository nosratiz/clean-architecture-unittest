using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hastnama.Solico.UnitTest.Common.ProductCategory
{
  public class ProductCategoryControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_ProductCategoryController_Return_OkResult()
        {

            var controller = new BaseConfiguration().BuildProductCategoryController();

            var result = await controller.Get(CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }
    }
}
