using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Delete;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Update;
using Hastnama.Solico.Application.Shop.ProductCategories.Dto;
using Hastnama.Solico.Application.Shop.ProductCategories.Queries;
using Hastnama.Solico.Application.UserManagement.Customers.Command.UpdateCustomer;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Queries;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.ProductCategory
{
   public class ProductCategoryControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_GetProductCategory_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildProductCategoryAdminController();
            var result = await controller.Get(new PagingOptions(), CancellationToken.None);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetProductCategoryInfo_Call_ReturnOkResult()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetProductCategoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductCategoryDto>.SuccessFul(
                    new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductCategoryAdminController();

            var result = await controller.GetProductCategoryInfo(It.IsAny<int>(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetProductCategoryInfo_Call_BadRequestResult()
        {
            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;
            
            mockData.Setup(x => x.Send(It.IsAny<GetProductCategoryQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<ProductCategoryDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                        await localization.GetMessage(ResponseMessage.CategoryNotFound, CancellationToken.None)))));
            
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductCategoryAdminController();
            
            var result = await controller.GetProductCategoryInfo(It.IsAny<int>(), CancellationToken.None);
            
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateProductCategory_ReturnNotContent()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateProductCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductCategoryAdminController();

            var result = await controller.Update(It.IsAny<UpdateProductCategoryCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateProductCategory_ReturnBadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateProductCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductCategoryAdminController();

            var result = await controller.Update(It.IsAny<UpdateProductCategoryCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteProductCategory_ReturnNotContent()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteProductCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductCategoryAdminController();

            var result = await controller.Delete(It.IsAny<DeleteProductCategoryCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteProductCategory_ReturnBadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteProductCategoryCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildProductCategoryAdminController();

            var result = await controller.Delete(It.IsAny<DeleteProductCategoryCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetListProductCategory_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildProductCategoryAdminController();
            var result = await controller.GetList(string.Empty, CancellationToken.None);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }
    }
}
