using System;
using System.Threading;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Shop.Companies.Command.Create;
using Hastnama.Solico.Application.Shop.Companies.Command.Delete;
using Hastnama.Solico.Application.Shop.Companies.Command.Update;
using Hastnama.Solico.Application.Shop.Companies.Dto;
using Hastnama.Solico.Application.Shop.Companies.Queires;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Common.Helper.Pagination;
using Hastnama.Solico.Common.Localization;
using Hastnama.Solico.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Company
{
    public class CompanyControllerTest : BaseConfiguration
    {
        [Fact]
        public async Task When_GetCompanyPagedListCall_ReturnOkResult()
        {
             var controller = new BaseConfiguration().BuildCompanyAdminController();

            var result = await controller.Get(new PagingOptions(), CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }


        [Fact]
        public async Task When_InvalidId_Send_BadRequestResult()
        {
            var mockData = new Mock<IMediator>();

            var localization = new Mock<ILocalization>().Object;

            mockData.Setup(x => x.Send(It.IsAny<GetCompanyQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    Result<CompanyDto>.Failed(
                        new BadRequestObjectResult(new ApiMessage(
                            await localization.GetMessage(ResponseMessage.CompanyNotFound, CancellationToken.None)))));

             var controller = new BaseConfiguration().WithMediatorService(mockData.Object)
                .BuildCompanyAdminController();


            var result = await controller.GetInfo(It.IsAny<int>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }


        [Fact]
        public async Task When_ValidId_Send_DeleteCompany_successfully()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<DeleteCompanyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);

             var controller = new BaseConfiguration().WithMediatorService(mockData.Object)
                .BuildCompanyAdminController();

            var result = await controller.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }
        
        
        [Fact]
        public async Task When_ValidData_send_ForUpdate_Successfully()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<UpdateCompanyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.SuccessFul);


             var controller = new BaseConfiguration().WithMediatorService(mockData.Object)
                .BuildCompanyAdminController();

            var result = await controller.Update(It.IsAny<UpdateCompanyCommand>(), It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<NoContentResult>(result);

            Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
        }
        
        
        [Fact]
        public async Task When_ValidData_Send_ForCreate_AddSuccessfully()
        {
            var mockData = new Mock<IMediator>();

            mockData.Setup(x => x.Send(It.IsAny<CreateCompanyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<CompanyDto>
                    .SuccessFul(new CompanyDto {Name = "Company",CreateDate = DateTime.Now,Slug = "company",Id = 1}));

             var controller = new BaseConfiguration().WithMediatorService(mockData.Object)
                .BuildCompanyAdminController();

            var result =
                await controller.Create(
                    It.IsAny<CreateCompanyCommand>(),
                    It.IsAny<CancellationToken>());

            var objectResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);
        }
    }
}