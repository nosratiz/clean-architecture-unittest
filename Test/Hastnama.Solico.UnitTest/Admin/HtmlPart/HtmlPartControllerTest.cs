using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.HtmlParts.Command.Create;
using Hastnama.Solico.Application.Cms.HtmlParts.Command.Update;
using Hastnama.Solico.Application.Cms.HtmlParts.Dto;
using Hastnama.Solico.Application.Cms.HtmlParts.Queries;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.HtmlPart
{
   public class HtmlPartControllerTest
    {
        [Fact]
        public async Task When_GetHtmlPartControllerCall_ReturnOkResult()
        {

            var controller = new BaseConfiguration().BuildHtmlPartAdminController();

            var result = await controller.Get(new PagingOptions(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetInfoHtmlPartControllerCall_ReturnOkResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetHtmlPartQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<HtmlPartDto>.SuccessFul(new OkObjectResult(new OkResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildHtmlPartAdminController();

            var result = await controller.GetInfo(It.IsAny<long>(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetInfoHtmlPartController_ReturnBadRequestResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetHtmlPartQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<HtmlPartDto>.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildHtmlPartAdminController();

            var result = await controller.GetInfo(It.IsAny<long>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateHtmlPartController_ReturnSuccessFully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateHtmlPartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<HtmlPartDto>.SuccessFul(new HtmlPartDto()));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildHtmlPartAdminController();

            var result = await controller.Create(It.IsAny<CreateHtmlPartCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateHtmlPartController_ReturnBadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateHtmlPartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<HtmlPartDto>.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildHtmlPartAdminController();

            var result = await controller.Create(It.IsAny<CreateHtmlPartCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateHtmlPartController_ReturnNotContent_SuccessFully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateHtmlPartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildHtmlPartAdminController();

            var result = await controller.Update(new UpdateHtmlPartCommand(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateHtmlPartController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<UpdateHtmlPartCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage(localization.GetMessage(ResponseMessage.HtmlPartNotFound)))));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildHtmlPartAdminController();

            var result = await controller.Update(It.IsAny<UpdateHtmlPartCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }



    }
}
