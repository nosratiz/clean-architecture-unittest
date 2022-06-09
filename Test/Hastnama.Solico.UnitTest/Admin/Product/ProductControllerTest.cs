using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Products.Command.Update;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Application.Shop.Products.Queries;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Product
{
  public class ProductControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_GetProduct_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildProductAdminController();

            var result = await controller.Get(new GetProductPagedListQuery(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }
        [Fact]
        public async Task When_GetProductExcelReport_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildProductAdminController();

            var result = await controller.GetExcelReport(new GetProductListQuery(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetOrderInfo_BadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetAdminProductQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductDto>.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductAdminController();

            var result = await controller.GetInfo(It.IsAny<long>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
        [Fact]
        public async Task When_GetProductInfo_ReturnOkResult()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetAdminProductQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductDto>.SuccessFul(new OkObjectResult(new OkResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductAdminController();

            var result = await controller.GetInfo(It.IsAny<long>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

    
        [Fact]
        public async Task When_UpdateProduct_BadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductAdminController();

            var result = await controller.Update(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateProduct_NotContent_Successfully()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductAdminController();

            var result = await controller.Update(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }
      

       
    }
}
