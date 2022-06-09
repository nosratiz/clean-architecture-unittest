using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Auth.Command.ChangePassword;
using Hastnama.Solico.Application.Auth.Command.ForgetPassword;
using Hastnama.Solico.Application.Auth.Command.Login;
using Hastnama.Solico.Application.Auth.Command.ResetPassword;
using Hastnama.Solico.Application.Auth.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Command.ConfirmCode;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Login;
using Hastnama.Solico.Application.UserManagement.Customers.Command.SendConfirmCode;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Queries;
using Hastnama.Solico.Application.UserManagement.Users.Command.UpdateProfile;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using Hastnama.Solico.Domain.Models.UserManagement;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Common.Account
{
  public class AccountControllerTest :BaseConfiguration
    {

        [Fact]
        public async Task When_LoginAccountController_Return_OkResult_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AuthResult>.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.Login(It.IsAny<LoginCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_LoginAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AuthResult>.Failed(new BadRequestObjectResult(new ApiMessage(localization.GetMessage(ResponseMessage.UserNotFound)))));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.Login(It.IsAny<LoginCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CustomerLoginAccountController_Return_OkResult_Successfully()
        {

            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AuthResult>.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.CustomerLogin(It.IsAny<CustomerLoginCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CustomerAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<CustomerLoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AuthResult>.Failed(new BadRequestObjectResult(new ApiMessage(localization.GetMessage(ResponseMessage.UserNotFound)))));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.CustomerLogin(It.IsAny<CustomerLoginCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_SendConfirmCodeAccountController_Return_NoContent_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<SendConfirmCodeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.SendConfirmCode(It.IsAny<SendConfirmCodeCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_SendConfirmCodeAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<SendConfirmCodeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.SendConfirmCode(It.IsAny<SendConfirmCodeCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ConfirmCodeAccountController_Return_OkResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<ConfirmCodeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AuthResult>.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.ConfirmCode(It.IsAny<ConfirmCodeCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ConfirmCodeAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<ConfirmCodeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AuthResult>.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.ConfirmCode(It.IsAny<ConfirmCodeCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ForgetPasswordAccountController_Return_OkResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<ForgetPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.ForgetPassword(It.IsAny<ForgetPasswordCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ForgetPasswordAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<ConfirmCodeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<AuthResult>.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.ConfirmCode(It.IsAny<ConfirmCodeCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ChangePasswordAccountController_Return_NoContent_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<ChangePasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.ChangePassword(It.IsAny<ChangePasswordCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ChangePasswordAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<ChangePasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.ChangePassword(It.IsAny<ChangePasswordCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ResetPasswordAccountController_Return_NoContent_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.ResetPassword(It.IsAny<ResetPasswordCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ResetPasswordAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.ResetPassword(It.IsAny<ResetPasswordCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateProfileAccountController_Return_NoContent_Successfully()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateProfileCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<User>.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.UpdateProfile(It.IsAny<UpdateProfileCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_UpdateProfileAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateProfileCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<User>.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.UpdateProfile(It.IsAny<UpdateProfileCommand>(), CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_MyProfileAccountController_Return_OkResult()
        {
            var controller = new BaseConfiguration().BuildAccountController();

            var result = await controller.MyProfile(CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_MyCreditAccountController_Return_OkResult()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetCustomerCreditQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CustomerCreditDto>.SuccessFul(new OkObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.MyCredit(CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_MyCreditAccountController_Return_BadRequest()
        {

            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<GetCustomerCreditQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CustomerCreditDto>.Failed(new BadRequestObjectResult(new ApiMessage())));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildAccountController();

            var result = await controller.MyCredit(CancellationToken.None);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

    }
}
