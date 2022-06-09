using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Products.Queries;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ProductDto = Hastnama.Solico.Application.Shop.Products.Dto.ProductDto;

namespace Hastnama.Solico.UnitTest.Common.Product
{
  public class ProductControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_GetProductController_Return_okResult()
        {

            var controller = new BaseConfiguration().BuildProductController();

            var result = await controller.Get(It.IsAny<GetProductForCustomerQuery>(),It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetInfoProductController_Return_okResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetProductQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductDto>.SuccessFul(new OkObjectResult(new ProductDto())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductController();

            var result = await controller.GetInfo(It.IsAny<long>(),It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetInfoProductController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();
            
            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<GetProductQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductDto>.Failed(new BadRequestObjectResult(new ApiMessage(
                    await localization.GetMessage(ResponseMessage.ProductNotFound, CancellationToken.None)))));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductController();

            var result = await controller.GetInfo(It.IsAny<int>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetMostVisitedProductController_Return_okResult()
        {

            var controller = new BaseConfiguration().BuildProductController();

            var result = await controller.GetMostVisitedProductByCustomer(It.IsAny<int>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetNewProductListProductController_Return_okResult()
        {

            var controller = new BaseConfiguration().BuildProductController();

            var result = await controller.GetNewProductList(It.IsAny<int>(),It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetProductBaseOnOrderProductController_Return_okResult()
        {

            var controller = new BaseConfiguration().BuildProductController();

            var result = await controller.GetProductBasedOnOrder(It.IsAny<int>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

    }
}
