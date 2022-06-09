using Hastnama.Solico.Api.Areas.Admin;
using Hastnama.Solico.Api.Areas.Common;
using MediatR;
using Moq;
using CompanyController = Hastnama.Solico.Api.Areas.Admin.CompanyController;
using ContactUsAdminController = Hastnama.Solico.Api.Areas.Admin.ContactUsController;
using FaqsAdminController = Hastnama.Solico.Api.Areas.Admin.FaqsController;
using ProductCategoryAdminController = Hastnama.Solico.Api.Areas.Admin.ProductCategoryController;
using ProductAdminController = Hastnama.Solico.Api.Areas.Admin.ProductController;
using SlideShowsAdminController = Hastnama.Solico.Api.Areas.Admin.SlideShowsController;
using SubscribersAdminController = Hastnama.Solico.Api.Areas.Admin.SubscribersController;
using ContactUsController = Hastnama.Solico.Api.Areas.Common.ContactUsController;
using FaqsController = Hastnama.Solico.Api.Areas.Common.FaqsController;
using ProductCategoryController = Hastnama.Solico.Api.Areas.Common.ProductCategoryController;
using ProductController = Hastnama.Solico.Api.Areas.Common.ProductController;
using SlideShowsController = Hastnama.Solico.Api.Areas.Common.SlideShowsController;
using SubscribersController = Hastnama.Solico.Api.Areas.Common.SubscribersController;

namespace Hastnama.Solico.UnitTest
{
    public class BaseConfiguration
    {
        private IMediator _mediator;

        public BaseConfiguration()
        {
            _mediator = new Mock<IMediator>().Object;
        }


        internal BaseConfiguration WithMediatorService(IMediator mediator)
        {
            _mediator = mediator;
            return this;
        }

        internal CompanyController BuildCompanyAdminController() => new(_mediator);
       
        internal ContactUsAdminController BuildContactUsAdminController() => new(_mediator);
       
        internal UserController BuildUserAdminController() => new (_mediator);
       
        internal CustomersController BuildCustomerAdminController() => new(_mediator);

        internal ProductCategoryAdminController BuildProductCategoryAdminController() => new (_mediator);

        internal OrdersController BuildOrdersAdminController() => new(_mediator);

        internal ProductAdminController BuildProductAdminController() => new(_mediator);

        internal SlideShowsAdminController BuildSlideShowsAdminController() => new(_mediator);

        internal HtmlPartController BuildHtmlPartAdminController() => new(_mediator);

        internal DashboardController BuildDashboardAdminController() => new(_mediator);

        internal FaqsAdminController BuildFaqsAdminController() => new(_mediator);

        internal SubscribersAdminController BuildSubscribersAdminController() => new (_mediator);

        internal AccountController BuildAccountController() => new (_mediator);

        internal CartController BuildCartController() => new(_mediator);

        internal ContactUsController BuildContactUsController() => new(_mediator);

        internal FaqsController BuildFaqsController() => new(_mediator);

        internal FilesController BuildFilesController() => new(_mediator);

        internal ProductCategoryController BuildProductCategoryController() => new(_mediator);

        internal ProductController BuildProductController() => new(_mediator);

        internal SettingController BuildSettingController() => new(_mediator);

        internal SlideShowsController BuildSlideShowController() => new(_mediator);

        internal SubscribersController BuildSubscribersController() => new(_mediator);
    }

}