using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Activation;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Create;
using Hastnama.Solico.Application.UserManagement.Customers.Command.UpdateCustomer;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Queries;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Customer
{
    public class CustomerControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_GetCustomers_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildCustomerAdminController();
          
            var result = await controller.Get(new GetCustomerPagedListQuery(), CancellationToken.None);
          
            var objectResult = Assert.IsType<OkObjectResult>(result);
          
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetExcelReport_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildCustomerAdminController();
          
            var result = await controller.GetExcelReport(CancellationToken.None);
         
            var objectResult = Assert.IsType<OkObjectResult>(result);
          
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdatePassword_BadRequest()
        {
            var mockData = new Mock<IMediator>();
          
            mockData.Setup(x => x.Send(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new BadRequestResult())));
          
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCustomerAdminController();
          
            var result = await controller.UpdatePassword(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>());
          
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
           
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CustomerUpdatePassword_NotContent()
        {
            var mockData = new Mock<IMediator>();
           
            mockData.Setup(x => x.Send(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCustomerAdminController();
           
            var result = await controller.UpdatePassword(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>());
           
            var objectResult = Assert.IsType<NoContentResult>(result);
           
            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CustomerGetInfo_Call_ReturnOkResult()
        {
            var mockData = new Mock<IMediator>();
          
            mockData.Setup(x => x.Send(It.IsAny<GetCustomerQuery>(), CancellationToken.None))
                .ReturnsAsync(Result<CustomerDto>.SuccessFul(new CustomerDto()));
          
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCustomerAdminController();
          
            var result = await controller.GetInfo(It.IsAny<string>(), CancellationToken.None);
          
            var objectResult = Assert.IsType<OkObjectResult>(result);
          
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CustomerGetInfo_BadRequestResult()
        {
            var mockData = new Mock<IMediator>();
           
            var localization = new Mock<ILocalization>().Object;
           
            mockData.Setup(x => x.Send(It.IsAny<GetCustomerQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CustomerDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                        await localization.GetMessage(ResponseMessage.ContentNotFound, CancellationToken.None)))));
          
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCustomerAdminController();
          
            var result = await controller.GetInfo(It.IsAny<string>(), CancellationToken.None);
          
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
          
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CustomerCreate_SuccessResult()
        {
            var mockData = new Mock<IMediator>();
          
            mockData.Setup(x => x.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(Result<CustomerEnrollmentDto>.SuccessFul(new CustomerEnrollmentDto()));
         
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCustomerAdminController();
            
            var result = await controller.Create(new CreateCustomerCommand(), It.IsAny<CancellationToken>());
          
            var objectResult = Assert.IsType<CreatedAtActionResult>(result);
          
            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CustomerCreate_BadRequest_CustomerId_Require()
        {
          
            var mockData = new Mock<IMediator>();
          
            var localization = new Mock<ILocalization>().Object;
          
            mockData.Setup(x => x.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CustomerEnrollmentDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                        await localization.GetMessage(ResponseMessage.SolicoCustomerIdIsRequired, CancellationToken.None)))));
          
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildCustomerAdminController();
           
            var result = await controller.Create(new CreateCustomerCommand(), CancellationToken.None);
           
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
           
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_GetCustomerEnrollment_Call_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildCustomerAdminController();
           
            var result = await controller.GetCustomerEnrollment(new GetCustomerEnrollmentPagedListQuery(), CancellationToken.None);
           
            var objectResult = Assert.IsType<OkObjectResult>(result);
          
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ValidId_Send_CustomerActivation_ReturnSuccess()
        {
            var mockData = new Mock<IMediator>();
           
            mockData.Setup(x => x.Send(It.IsAny<ActivationCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object)
                .BuildCustomerAdminController();
           
            var result = await controller.Activation(new Guid(), It.IsAny<CancellationToken>());
           
            var objectResult = Assert.IsType<NoContentResult>(result);
           
            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_InvalidId_Send_CustomerActivation_ReturnBadRequest()
        {
            var mockData = new Mock<IMediator>();
           
            var localization = new Mock<ILocalization>().Object;
           
            mockData.Setup(x => x.Send(It.IsAny<ActivationCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                            await localization.GetMessage(ResponseMessage.ContentNotFound, CancellationToken.None)))));
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object)
                .BuildCustomerAdminController();
           
            var result = await controller.Activation(new Guid(), It.IsAny<CancellationToken>());
            
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

    }
}
