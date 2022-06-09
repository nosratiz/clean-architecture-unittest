using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Cms.ContactUses.Command.Delete;
using Hastnama.Solico.Application.Cms.ContactUses.Dto;
using Hastnama.Solico.Application.Cms.ContactUses.Queries;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.ContactUs
{
    public class ContactUsControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_GetContactUsCall_ReturnOkResult()
        {
            var controller = new BaseConfiguration().BuildContactUsAdminController();

            var result = await controller.Get(new PagingOptions(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }


        [Fact]
        public async Task When_InvalidId_Send_BadRequestResult()
        {
            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<GetContactUsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<ContactUsDto>.Failed(
                        new BadRequestObjectResult(new ApiMessage(
                            await localization.GetMessage(ResponseMessage.ContentNotFound, CancellationToken.None)))));

             var controller = new BaseConfiguration().WithMediatorService(mockData.Object)
                .BuildContactUsAdminController();


            var result = await controller.GetInfo(It.IsAny<int>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }


        [Fact]
        public async Task When_ValidId_Send_DeleteContact_successfully()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteContactUsCommand>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

             var controller = new BaseConfiguration().WithMediatorService(mockData.Object)
                .BuildContactUsAdminController();

            var result = await controller.Delete(It.IsAny<DeleteContactUsCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }
    }
}