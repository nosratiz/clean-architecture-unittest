using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.ContactUses.Command.Create;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Common.ContactUs
{
   public class ContactUsControllerTest :BaseConfiguration
    {
        [Fact]
        public async Task When_CreateContactUsController_Return_NotContent_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateContactUsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildContactUsController();

            var result = await controller.Create(It.IsAny<CreateContactUsCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateContactUsController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateContactUsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildContactUsController();

            var result = await controller.Create(It.IsAny<CreateContactUsCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
    }
}
