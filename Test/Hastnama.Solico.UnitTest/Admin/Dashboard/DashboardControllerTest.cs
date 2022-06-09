using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Hastnama.Solico.UnitTest.Admin.Dashboard
{
   public class DashboardControllerTest
    {
        [Fact]
        public async Task When_GetDashboardControllerCall_ReturnOkResult()
        {

            var controller = new BaseConfiguration().BuildDashboardAdminController();

            var result = await controller.Get(CancellationToken.None);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

    }
}
