using AutoMapper;
using Hastnama.Solico.Application.Cms.ContactUses.Command.Create;
using Hastnama.Solico.Application.Cms.ContactUses.Dto;
using Hastnama.Solico.Application.Cms.Faqs.Command.Create;
using Hastnama.Solico.Application.Cms.Faqs.Command.Update;
using Hastnama.Solico.Application.Cms.Faqs.Dto;
using Hastnama.Solico.Application.Cms.Settings.Command.UpdateSetting;
using Hastnama.Solico.Application.Cms.Settings.Dto;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Create;
using Hastnama.Solico.Application.Cms.SlideShows.Command.Update;
using Hastnama.Solico.Application.Cms.SlideShows.Dto;
using Hastnama.Solico.Application.Files.Dto;
using Hastnama.Solico.Application.Shop.Products.Command.Update;
using Hastnama.Solico.Application.Shop.Products.Dto;
using Hastnama.Solico.Application.Statistics.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Application.UserManagement.Users.Command.CreateUser;
using Hastnama.Solico.Application.UserManagement.Users.Command.UpdateProfile;
using Hastnama.Solico.Application.UserManagement.Users.Command.UpdateUser;
using Hastnama.Solico.Application.UserManagement.Users.ModelDto;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Domain.Models.Cms;
using Hastnama.Solico.Domain.Models.Shop;
using Hastnama.Solico.Domain.Models.Statistic;
using Hastnama.Solico.Domain.Models.UserManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Hastnama.Solico.Application.Cms.CustomerConsults.Command.Create;
using Hastnama.Solico.Application.Cms.CustomerConsults.Dto;
using Hastnama.Solico.Application.Cms.Messages.Command.ReplyCustomerMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.ReplyUserMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.SendCustomerMessage;
using Hastnama.Solico.Application.Cms.Messages.Command.SendUserMessage;
using Hastnama.Solico.Application.Cms.Messages.Dto;
using Hastnama.Solico.Application.Cms.Subscribers.Command.Create;
using Hastnama.Solico.Application.Cms.Subscribers.Dto;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.Shop.Cart.Command;
using Hastnama.Solico.Application.Shop.Cart.Command.Create;
using Hastnama.Solico.Application.Shop.Cart.Dto;
using Hastnama.Solico.Application.Shop.Companies.Command.Create;
using Hastnama.Solico.Application.Shop.Companies.Command.Update;
using Hastnama.Solico.Application.Shop.Companies.Dto;
using Hastnama.Solico.Application.Shop.Orders.Command;
using Hastnama.Solico.Application.Shop.Orders.Dto;
using Hastnama.Solico.Application.Shop.Payments.Dto;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Create;
using Hastnama.Solico.Application.Shop.ProductCategories.Command.Update;
using Hastnama.Solico.Application.Shop.ProductCategories.Dto;
using Hastnama.Solico.Application.UserManagement.Customers.Command.Create;
using Hastnama.Solico.Common.Enums;

namespace Hastnama.Solico.Application.Common.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User

            CreateMap<User, UserDto>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(des => $"{des.Name} {des.Family}"));

            CreateMap<CreateUserCommand, User>()
                .ForMember(x => x.ExpiredVerification, opt => opt.MapFrom(des => DateTime.Now.AddDays(1)))
                .ForMember(x => x.ActivationCode, opt => opt.MapFrom(des => new Random().Next(100000, 999999)))
                .ForMember(x => x.RegisterDate, opt => opt.MapFrom(des => DateTime.Now)).ForMember(x => x.Password,
                    opt => opt.MapFrom(des => PasswordManagement.HashPass(des.Password)));


            CreateMap<UpdateUserCommand, User>();

            CreateMap<UpdateProfileCommand, User>();

            #endregion

            #region Faq

            CreateMap<Faq, FaqDto>();

            CreateMap<CreateFaqCommand, Faq>();

            CreateMap<UpdateFaqCommand, Faq>();

            #endregion

            #region Contact Us

            CreateMap<ContactUs, ContactUsDto>();
            CreateMap<CreateContactUsCommand, ContactUs>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            #endregion

            #region App setting

            CreateMap<AppSetting, AppSettingDto>().ForMember(x => x.UserName,
                opt => opt.MapFrom(des => $"{des.User.Name} {des.User.Family}"));

            CreateMap<UpdateSettingCommand, AppSetting>();

            #endregion

            #region Slider

            CreateMap<SlideShow, SlidShowDto>();

            CreateMap<CreateSlidShowCommand, SlideShow>();

            CreateMap<UpdateSlideShowCommand, SlideShow>();

            #endregion

            #region File

            CreateMap<UserFile, FileDto>();

            #endregion

            #region Statistic

            CreateMap<DailyStatistic, DailyStatisticDto>();

            #endregion

            #region Customers

            CreateMap<ItCreditExposureDto, CustomerCreditDto>()
                .ForMember(x => x.Currency, opt => opt.MapFrom(des => des.CMWAE))
                .ForMember(x => x.Exposure, opt => opt.MapFrom(des => des.EXPOSURE))
                .ForMember(x => x.Limit, opt => opt.MapFrom(des => des.LIMIT))
                .ForMember(x => x.CreditLimitUsed, opt => opt.MapFrom(des => des.USED));

            CreateMap<SolicoCustomerDto, Customer>().ForMember(x=>x.PayerId,opt=>opt.MapFrom(des=>des.KUNN2))
                .ForMember(x => x.FullName, opt => opt.MapFrom(des =>
                    $"{des.NAME1} {des.NAME2} {des.NAME3} {des.NAME4}"))
                .ForMember(x => x.SolicoCustomerId, opt => opt.MapFrom(des => des.KUNNR))
                .ForMember(x => x.Country, opt => opt.MapFrom(des => des.LAND1))
                .ForMember(x => x.CityCode, opt => opt.MapFrom(des => des.CITYC))
                .ForMember(x => x.Phone, opt => opt.MapFrom(des => des.ZZNOTE_1))
                .ForMember(x => x.Mobile, opt => opt.MapFrom(des => des.ZZNOTE_2))
                .ForMember(x => x.Address,
                    opt => opt.MapFrom(des => $"{des.STR_SUPPL1} {des.STR_SUPPL2} {des.STR_SUPPL3}"))
                .ForMember(x => x.SyncDate, opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.Region, opt => opt.MapFrom(des => des.REGIO))
                .ForMember(x => x.AddressNumber, opt => opt.MapFrom(des => des
                    .ADDRNUMBER));
            
            CreateMap<UpdateSolicoCustomerDto, Customer>()
                .ForMember(x=>x.PayerId,opt=>opt.MapFrom(des=>des.KUNN2))
                .ForMember(x => x.FullName, opt => opt.MapFrom(des =>
                    $"{des.NAME1} {des.NAME2} {des.NAME3} {des.NAME4}"))
                .ForMember(x => x.SolicoCustomerId, opt => opt.MapFrom(des => des.KUNNR))
                .ForMember(x => x.Country, opt => opt.MapFrom(des => des.LAND1))
                .ForMember(x => x.CityCode, opt => opt.MapFrom(des => des.CITYC))
                .ForMember(x => x.Phone, opt => opt.MapFrom(des => des.ZZNOTE_1))
                .ForMember(x => x.Address,
                    opt => opt.MapFrom(des => $"{des.STR_SUPPL1} {des.STR_SUPPL2} {des.STR_SUPPL3}"))
                .ForMember(x => x.SyncDate, opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.Region, opt => opt.MapFrom(des => des.REGIO))
                .ForMember(x => x.AddressNumber, opt => opt.MapFrom(des => des
                    .ADDRNUMBER));


            CreateMap<SolicoCustomerDto, UpdateSolicoCustomerDto>();



            CreateMap<Customer, CustomerDto>();

            CreateMap<CustomerEnrollment, CustomerEnrollmentDto>();

            CreateMap<CreateCustomerCommand, CustomerEnrollment>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            #endregion

            #region Product

            CreateMap<ProductMedia, ProductMediaDto>();

            CreateMap<UpdateProductMediaDto, ProductMedia>();

            CreateMap<SolicoMaterialDto, Product>().ForMember(x=>x.Division,opt=>opt.MapFrom(des=>$"{des.SPART} - {des.VTEXT}"))
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.MaterialId, opt => opt.MapFrom(des => des.MATNR))
               .ForMember(x => x.Name, opt => opt.MapFrom(des => des.MAKTX))
                .ForMember(x => x.Description, opt => opt.MapFrom(des => des.WGBEZ60))
                .ForMember(x => x.Lang, opt => opt.MapFrom(des => "fa"))
                .ForMember(x => x.Tag, opt => opt.MapFrom(des => "[]"))
                .ForMember(x => x.Unit, opt => opt.MapFrom(des => des.MEINS));


            CreateMap<Product, ProductDto>().ForMember(x=>x.MaterialId,opt=>opt.MapFrom(des=>des.MaterialId.TrimStart('0')))
                .ForMember(x => x.Price,
                    opt => opt.MapFrom(
                        des => des.CustomerProductPrices.FirstOrDefault(x => x.ProductId == des.Id).Price))
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(des => des.ProductCategory.Name)).ForMember(
                    x => x.Tag, opt => opt.MapFrom(des =>
                        JsonConvert.DeserializeObject<List<string>>(des.Tag)))
                .ForMember(x => x.Galleries, opt =>
                    opt.MapFrom(des => des.ProductGalleries.Select(x => x.Image).ToList()));

            CreateMap<UpdateProductCommand, Product>()
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.Tag, opt => opt.MapFrom(des => JsonConvert.SerializeObject(des.Tag)));

            #endregion

            #region productCategory

            CreateMap<ProductCategory, ProductCategoryDto>();

            CreateMap<CreateProductCategoryCommand, ProductCategory>();

            CreateMap<UpdateProductCategoryCommand, ProductCategory>();

            CreateMap<CreateChildrenCategoryCommand, ProductCategory>();

            CreateMap<ProductCategory, AdminProductCategoryDto>().ForMember(x => x.Name, opt
                => opt.MapFrom(des => $"{des.Parent.Name} > {des.Name}"));

            #endregion

            #region Company

            CreateMap<Company, CompanyDto>();

            CreateMap<CreateCompanyCommand, Company>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));


            CreateMap<UpdateCompanyCommand, Company>();

            #endregion

            #region Subscribe

            CreateMap<Subscribe, SubscriberDto>();

            CreateMap<CreateSubscriberCommand, Subscribe>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(des => true));

            #endregion

            #region OrderItem

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.ProductName, opt => opt.MapFrom(des => des.Product.Name))
                .ForMember(x => x.ProductImage, opt => opt.MapFrom(des => des.Product.Image));

            CreateMap<OrderItem, HistoryOrderItem>()
                .ForMember(x => x.ProductName, opt => opt.MapFrom(des => des.Product.Name))
                .ForMember(x => x.ProductImage, opt => opt.MapFrom(des => des.Product.Image));

            CreateMap<HistoryOrderItem, OrderItemDto>();
            
            CreateMap<CreateCardCommand, OrderItem>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Now));

            #endregion

            #region Order

            CreateMap<OrderStatusHistory, OrderStatusHistoryDto>();

            CreateMap<Order, OrderDto>();

            CreateMap<SubmitOrderCommand, Order>()
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime
                    .Now)).ForMember(x => x.OrderNumber,
                    opt => opt.MapFrom(des => $"Slc-{new Random().Next(Int32.MinValue, int.MaxValue)}"))
                .ForMember(x => x.OrderStatus, opt => opt.MapFrom(des => (int) OrderStatus.Pending))
                .ForMember(x => x.ShipmentPrice, opt => opt.MapFrom(des => 0))
                .ForMember(x => x.DiscountPrice, opt => opt.MapFrom(des => 0));

            #endregion

            #region SolicoSailesOrder

            CreateMap<SolicoOrderDetailDto, SailedOrderDetailDto>()
                .ForMember(x => x.MaterialId, opt => opt.MapFrom(des => des.MATNR))
                .ForMember(x => x.Count, opt => opt.MapFrom(des => des.KWMENG))
                .ForMember(x => x.Unit, opt => opt.MapFrom(des => des.VRKME))
                .ForMember(x => x.UnitPrice, opt => opt.MapFrom(des => des.NET_PRICE))
                .ForMember(x => x.Tax, opt => opt.MapFrom(des => des.TAX))
                .ForMember(x => x.Levy, opt => opt.MapFrom(des => des.LEVY))
                .ForMember(x => x.FinalPrice, opt => opt.MapFrom(des => des.NETWR));


            CreateMap<SolicoOrderDto, SailedOrderDto>()
                .ForMember(x => x.QuotationNumber, opt => opt.MapFrom(des => des.VBELV))
                .ForMember(x => x.OrderNumber, opt => opt.MapFrom(des => des.VBELN))
                .ForMember(x => x.SolicoCustomerId, opt => opt.MapFrom((des => des.KUNNR)))
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Parse(des.AUDAT)))
                .ForMember(x => x.OrderDetail, opt => opt.MapFrom((des => des.ORDER_DETAILS)));

            CreateMap<ProformaDto, SailedOrderDto>()
                .ForMember(x => x.QuotationNumber, opt => opt.MapFrom(des => des.VBELV))
                .ForMember(x => x.OrderNumber, opt => opt.MapFrom(des => des.VBELN))
                .ForMember(x => x.SolicoCustomerId, opt => opt.MapFrom((des => des.KUNNR)))
                .ForMember(x => x.CreateDate, opt => opt.MapFrom(des => DateTime.Parse(des.FKDAT)))
                .ForMember(x => x.OrderDetail, opt => opt.MapFrom((des => des.PROFORMA_DETAILS)));


            CreateMap<SolicoProformaDetailDto, SailedOrderDetailDto>()
                .ForMember(x => x.MaterialId, opt => opt.MapFrom(des => des.MATNR))
                .ForMember(x => x.Count, opt => opt.MapFrom(des => des.FKIMG))
                .ForMember(x => x.Unit, opt => opt.MapFrom(des => des.VRKME))
                .ForMember(x => x.UnitPrice, opt => opt.MapFrom(des => des.NET_PRICE))
                .ForMember(x => x.Tax, opt => opt.MapFrom(des => des.TAX))
                .ForMember(x => x.Levy, opt => opt.MapFrom(des => des.LEVY))
                .ForMember(x => x.FinalPrice, opt => opt.MapFrom(des => des.NETWR));

            #endregion

            #region Message

            CreateMap<SendCustomerMessageCommand, Message>()
                .ForMember(x => x.CreateDate,
                    opt => opt.MapFrom(des => DateTime.Now));


            CreateMap<SendUserMessageCommand, Message>()
                .ForMember(x => x.CreateDate,
                    opt => opt.MapFrom(des => DateTime.Now));


            CreateMap<ReplyUserMessageCommand, Message>()
                .ForMember(x => x.CreateDate,
                    opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.ParentId, opt => opt.MapFrom(des => des.ParentMessageId));


            CreateMap<ReplyCustomerMessageCommand, Message>()
                .ForMember(x => x.CreateDate,
                    opt => opt.MapFrom(des => DateTime.Now))
                .ForMember(x => x.ParentId, opt => opt.MapFrom(des => des.ParentMessageId));


            CreateMap<UserMessage, UserMessageDto>()
                .ForMember(x => x.Content, opt => opt.MapFrom(des => des.Message.Content))
                .ForMember(x => x.Title, opt => opt.MapFrom(des => des.Message.Title))
                .ForMember(x => x.File, opt => opt.MapFrom((des => des.Message.File)))
                .ForMember(x => x.UserName, opr => opr
                    .MapFrom(des => $"{des.User.Name} {des.User.Family}"))
                .ForMember(x => x.CustomerName, opt => opt.MapFrom(des => des.Customer.FullName));

            #endregion

            #region Consult

            CreateMap<CustomerConsult, CustomerConsultDto>().ForMember(x=>x.CustomerPhone,opt=>opt.MapFrom((des=>$"{des.Customer.Mobile} - { des.Customer.Phone}")))
                .ForMember(x => x.CustomerName, 
                    opt => opt.MapFrom(des => des.Customer.FullName));


            CreateMap<CreateCustomerConsultCommand, CustomerConsult>()
                .ForMember(x => x.CreateDate, 
                    opt => opt.MapFrom(des => DateTime.Now));

            CreateMap<CustomerOpenItem, CustomerOpenItemDto>();

            CreateMap<openItem, CustomerOpenItem>()
                .ForMember(x => x.DocumentNumber, opt => opt.MapFrom(des => des.BELNR))
                .ForMember(x => x.Amount, opt => opt.MapFrom(des => des.DMBTR))
                .ForMember(x => x.DueDate, opt => opt.MapFrom(des => DateTime.Parse(des.ZFBDT).AddDays(des.ZBD1T)));    

            #endregion

            #region transaction

            CreateMap<BankTransaction, BankTransactionDto>().ForMember(x => x.DocumentNumber,
                    opt => opt.MapFrom(des => des.CustomerOpenItem.DocumentNumber))
                .ForMember(x => x.OrderNumber, opt => opt.MapFrom(des => des.Order.OrderNumber));

            #endregion
        }
    }
}