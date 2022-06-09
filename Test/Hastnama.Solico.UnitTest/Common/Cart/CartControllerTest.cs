using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Cart.Command.Create;
using Hastnama.Solico.Application.Shop.Cart.Command.Delete;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Common.Cart
{
   public class CartControllerTest : BaseConfiguration
    {

        [Fact]
        public async Task When_GetCartController_Return_OkResult()
        {
            var mockData = new Mock<IMediator>();

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCartController();

            var result = await controller.Get(CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteCartController_Return_NotContent_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteOrderItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCartController();

            var result = await controller.Delete(It.IsAny<Guid>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteCartController_Return_BadRequestResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteOrderItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCartController();

            var result = await controller.Delete(It.IsAny<Guid>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_AddToCart_CartController_Return_NotContent_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateCardCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCartController();

            var result = await controller.AddToCart(It.IsAny<CreateCardCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_AddToCart_CartController_Return_BadRequestResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteOrderItemCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCartController();

            var result = await controller.AddToCart(It.IsAny<CreateCardCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

    }
}
