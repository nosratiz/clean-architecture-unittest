using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Files.Command;
using Hastnama.Solico.Application.Files.Dto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Common.Files
{
  public  class FilesControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_CreateFilesController_Return_OkResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateFileCommand>(),CancellationToken.None))
                .ReturnsAsync(Result<FileDto>.SuccessFul(new FileDto()));


            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFilesController();

            var result = await controller.Create(It.IsAny<CreateFileCommand>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateFilesController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateFileCommand>(),CancellationToken.None))
                .ReturnsAsync(Result<FileDto>.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFilesController();

            var result = await controller.Create(It.IsAny<CreateFileCommand>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
    }
}
