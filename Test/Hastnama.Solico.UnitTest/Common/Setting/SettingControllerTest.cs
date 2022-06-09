using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Settings.Command.UpdateSetting;
using Hastnama.Solico.Application.Cms.Settings.Dto;
using Hastnama.Solico.Application.Cms.Settings.Queries;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Common.Setting
{
    public class SettingControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_GetSettingController_Call_ReturnOkResult()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetSettingQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AppSettingDto>.SuccessFul(new OkObjectResult(new AppSettingDto())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSettingController();


            var result = await controller.Get(new CancellationToken());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateSettingController_BadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateSettingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSettingController();

            var result = await controller.Update(It.IsAny<UpdateSettingCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateSettingController_NotContent_Successfully()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateSettingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSettingController();

            var result = await controller.Update(It.IsAny<UpdateSettingCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }
    }
}