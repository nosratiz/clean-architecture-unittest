using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Create;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Delete;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Update;
using Hastnama.Solico.Application.Cms.SlideShows.Dto;
using Hastnama.Solico.Application.Cms.SlideShows.Queries;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.SlideShows
{
   public class SlideShowsControllerTest
    {
        [Fact]
        public async Task When_GetSlideShowsControllerCall_ReturnOkResult()
        {

            var controller = new BaseConfiguration().BuildSlideShowsAdminController();

            var result = await controller.Get(new PagingOptions(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ValidId_Send_GetSlideShowsController_ReturnSuccessfully()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetSlideShowQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SlidShowDto>.SuccessFul(new OkObjectResult(new OkResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSlideShowsAdminController();

            var result = await controller.GetSlideShow(It.IsAny<int>(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_InValidId_Send_GetSlideShowsController_ReturnBadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetSlideShowQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SlidShowDto>.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSlideShowsAdminController();

            var result = await controller.GetSlideShow(It.IsAny<int>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateSlideShowsController_ReturnSuccessfully()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateSlidShowCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SlidShowDto>.SuccessFul(new SlidShowDto()));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSlideShowsAdminController();

            var result = await controller.Create(It.IsAny<CreateSlidShowCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateSlideShowsController_ReturnBadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateSlidShowCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SlidShowDto>.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSlideShowsAdminController();

            var result = await controller.Create(It.IsAny<CreateSlidShowCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
        [Fact]
        public async Task When_UpdateSlideShowsController_NotContent_ReturnSuccess()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateSlideShowCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSlideShowsAdminController();

            var result = await controller.Update(It.IsAny<UpdateSlideShowCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateSlideShowsController_ReturnBadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateSlideShowCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSlideShowsAdminController();

            var result = await controller.Update(It.IsAny<UpdateSlideShowCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteSlideShowsController_NotContent_ReturnSuccess()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteSlideShowCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSlideShowsAdminController();

            var result = await controller.Delete(It.IsAny<DeleteSlideShowCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteSlideShowsController_ReturnBadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteSlideShowCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSlideShowsAdminController();

            var result = await controller.Delete(It.IsAny<DeleteSlideShowCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }



    }
}
