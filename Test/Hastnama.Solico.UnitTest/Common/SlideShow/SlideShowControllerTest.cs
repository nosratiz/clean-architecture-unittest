using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hastnama.Solico.UnitTest.Common.SlideShow
{
  public class SlideShowControllerTest:BaseConfiguration
    {
        [Fact]
        public async Task When_GetSlideShowController_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildSlideShowController();

            var result = await controller.Get(new CancellationToken());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }
    }
}
