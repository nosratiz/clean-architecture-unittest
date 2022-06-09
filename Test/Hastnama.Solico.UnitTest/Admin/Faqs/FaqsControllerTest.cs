using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Faqs.Command.Create;
using Hastnama.Solico.Application.Cms.Faqs.Command.Delete;
using Hastnama.Solico.Application.Cms.Faqs.Command.Update;
using Hastnama.Solico.Application.Cms.Faqs.Dto;
using Hastnama.Solico.Application.Cms.Faqs.Queries;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Faqs
{
   public class FaqsControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_GetFaqsControllerCall_ReturnOkResult()
        {

            var controller = new BaseConfiguration().BuildFaqsAdminController();

            var result = await controller.Get(new PagingOptions(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetFaqInfoController_Return_Success()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetFaqQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<FaqDto>.SuccessFul(new OkObjectResult(new OkResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFaqsAdminController();

            var result = await controller.GetFaqInfo(It.IsAny<int>(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }


        [Fact]
        public async Task When_GetFaqInfoController_ReturnBadRequestResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetFaqQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<FaqDto>.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFaqsAdminController();

            var result = await controller.GetFaqInfo(It.IsAny<int>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateFaqsController_ReturnSuccessfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateFaqCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<FaqDto>.SuccessFul(new FaqDto()));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFaqsAdminController();

            var result = await controller.Create(It.IsAny<CreateFaqCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateFaqsController_ReturnBadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateFaqCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<FaqDto>.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFaqsAdminController();

            var result = await controller.Create(It.IsAny<CreateFaqCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateFaqsController_Return_NotContent_Successfully()
        {

            var mockData = new Mock<IMediator>();


            mockData.Setup(x => x.Send(It.IsAny<UpdateFaqCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFaqsAdminController();

            var result = await controller.Update(It.IsAny<UpdateFaqCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateFaqsController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<UpdateFaqCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage(localization.GetMessage(ResponseMessage.FaqNotFound)))));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFaqsAdminController();

            var result = await controller.Update(It.IsAny<UpdateFaqCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteFaqsController_Return_NotContent_Successfully()
        {

            var mockData = new Mock<IMediator>();


            mockData.Setup(x => x.Send(It.IsAny<DeleteFaqCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFaqsAdminController();

            var result = await controller.Delete(It.IsAny<DeleteFaqCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteFaqsController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<DeleteFaqCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage(localization.GetMessage(ResponseMessage.FaqNotFound)))));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildFaqsAdminController();

            var result = await controller.Delete(It.IsAny<DeleteFaqCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
    }
}
