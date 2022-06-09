using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Orders.Dto;
using Hastnama.Solico.Application.Shop.Orders.Queries;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Orders
{
   public class OrdersControllerTest: BaseConfiguration
    {
        [Fact]
        public async Task When_GetOrders_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildOrdersAdminController();

            var result = await controller.Get(new GetOrderPagedListQuery() , CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }
        [Fact]
        public async Task When_GetOrdersExcelReport_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildOrdersAdminController();

            var result = await controller.GetExcelReport(new GetOrderListQuery(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetOrderInfo_BadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetOrderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<OrderDto>.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildOrdersAdminController();

            var result = await controller.GetOrderInfo(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
        [Fact]
        public async Task When_GetOrderInfo_ReturnOkResult()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetOrderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<OrderDto>.SuccessFul(new OkObjectResult(new OkResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildOrdersAdminController();

            var result = await controller.GetOrderInfo(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetSailedOrder_BadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetSailedOrderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SailedOrderDto>.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildOrdersAdminController();

            var result = await controller.GetSailedOrder(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetSailedOrder_ReturnOkResult()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetSailedOrderQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SailedOrderDto>.SuccessFul(new OkObjectResult(new OkResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildOrdersAdminController();

            var result = await controller.GetSailedOrder(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetInvoiceOrder_BadRequest()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetInvoiceQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SailedOrderDto>.Failed(new BadRequestObjectResult(new BadRequestResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildOrdersAdminController();

            var result = await controller.GetInvoice(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetInvoiceOrder_ReturnOkResult()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetInvoiceQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SailedOrderDto>.SuccessFul(new OkObjectResult(new OkResult())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildOrdersAdminController();

            var result = await controller.GetInvoice(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

    }
}
