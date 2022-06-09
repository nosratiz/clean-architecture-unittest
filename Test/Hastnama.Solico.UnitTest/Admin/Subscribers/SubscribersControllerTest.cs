using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.Faqs.Command.Delete;
using Hastnama.Solico.Application.Cms.Subscribers.Command.Delete;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Subscribers
{
  public class SubscribersControllerTest :BaseConfiguration
    {
        [Fact]
        public async Task When_GetSubscribersControllerCall_ReturnOkResult()
        {

            var controller = new BaseConfiguration().BuildSubscribersAdminController();

            var result = await controller.Get(new PagingOptions(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteSubscribersController_Return_NotContent_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteSubscriberCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSubscribersAdminController();

            var result = await controller.Delete(It.IsAny<DeleteSubscriberCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteSubscribersController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<DeleteSubscriberCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage(localization.GetMessage(ResponseMessage.FaqNotFound)))));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildSubscribersAdminController();

            var result = await controller.Delete(It.IsAny<DeleteSubscriberCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
    }
}
