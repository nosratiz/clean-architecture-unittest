using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.UserManagement.Users.Command.CreateUser;
using Hastnama.Solico.Application.UserManagement.Users.Command.DeleteUser;
using Hastnama.Solico.Application.UserManagement.Users.Command.UpdateUser;
using Hastnama.Solico.Application.UserManagement.Users.ModelDto;
using Hastnama.Solico.Application.UserManagement.Users.Queries;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.User
{
   public class UserControllerTest:BaseConfiguration
    {
        [Fact]
        public async Task When_GetUserControllerCall_ReturnOkResult()
        {
           
            var controller = new BaseConfiguration().BuildUserAdminController();
           
            var result = await controller.Get(new GetUserListQuery(), CancellationToken.None);
           
            var objectResult = Assert.IsType<OkObjectResult>(result);
            
            Assert.Equal(StatusCodes.Status200OK,objectResult.StatusCode);
        }

        [Fact]
        public async Task When_InvalidId_Send_GetInfoUserController_BadRequestResult()
        {
            var mockData = new Mock<IMediator>();
            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                        await localization.GetMessage(ResponseMessage.ContentNotFound, CancellationToken.None)))));

            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildUserAdminController();
            
            var result = await controller.GetInfo(It.IsAny<int>(), CancellationToken.None);
           
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
           
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_ValidId_Send_GetInfoUserController_SuccessResult()
        {
            var mockData = new Mock<IMediator>();
           
            mockData.Setup(x => x.Send(It.IsAny<GetUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserDto>.SuccessFul(new UserDto()));
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildUserAdminController();
           
            var result = await controller.GetInfo(It.IsAny<int>(), CancellationToken.None);
           
            var objectResult = Assert.IsType<OkObjectResult>(result);
           
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteCommand_Send_DeleteUserController_Success()
        {
            var mockData = new Mock<IMediator>();
           
            mockData.Setup(x => x.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildUserAdminController();
           
            var result = await controller.Delete(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>());
           
            var objectResult = Assert.IsType<NoContentResult>(result);
           
            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_DeleteCommand_Send_DeleteUserController_BadRequest()
        {
            var mockData = new Mock<IMediator>();
            
            var localization = new Mock<ILocalization>().Object;
            
            mockData.Setup(x => x.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                        await localization.GetMessage(ResponseMessage.ContentNotFound, CancellationToken.None)))));
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildUserAdminController();
           
            var result = await controller.Delete(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>());
           
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
           
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
      
        [Fact]
        public async Task When_UpdateCommand_Send_UpdateUserController_Success()
        {
            var mockData = new Mock<IMediator>();
           
            mockData.Setup(x => x.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildUserAdminController();
           
            var result = await controller.Update(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>());
           
            var objectResult = Assert.IsType<NoContentResult>(result);
           
            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }
        [Fact]
        public async Task When_UpdateCommand_Send_UpdateUserController_BadRequest()
        {
            var mockData = new Mock<IMediator>();
            
            var localization = new Mock<ILocalization>().Object;
           
            mockData.Setup(x => x.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                        await localization.GetMessage(ResponseMessage.ContentNotFound, CancellationToken.None)))));
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildUserAdminController();
           
            var result = await controller.Update(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>());
           
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
           
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateUserCommand_User_Send_Success()
        {
            var mockData = new Mock<IMediator>();
           
            mockData.Setup(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserDto>.SuccessFul(new UserDto()));
            
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildUserAdminController();
            
            var result = await controller.CreateUser(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>());
           
            var objectResult = Assert.IsType<CreatedAtActionResult>(result);
           
            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        }

        [Fact]
        public async Task When_CreateUserCommand_User_Send_BadRequest()
        {
            var mockData = new Mock<IMediator>();
           
            var localization = new Mock<ILocalization>().Object;
           
            mockData.Setup(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<UserDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(
                        await localization.GetMessage(ResponseMessage.ContentNotFound, CancellationToken.None)))));
           
            var controller = new BaseConfiguration().WithMediatorService(mockData.Object).BuildUserAdminController();
           
            var result = await controller.CreateUser(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>());
           
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
           
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }



    }
}
