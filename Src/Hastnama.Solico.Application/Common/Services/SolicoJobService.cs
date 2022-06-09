using System;
using System.Collections.Generic;
using AutoMapper;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Domain.Models.Shop;
using Hastnama.Solico.Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using Hastnama.Solico.Application.Common.SolicoWebServiceDto;
using Hastnama.Solico.Application.UserManagement.Customers.Dto;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Helper;
using Serilog;

namespace Hastnama.Solico.Application.Common.Services
{
    public class SolicoJobService : ISolicoJobService
    {
        private readonly ISolicoDbContext _context;
        private readonly ISolicoWebService _solicoWebService;
        private readonly IMapper _mapper;


        public SolicoJobService(ISolicoDbContext context, ISolicoWebService solicoWebService, IMapper mapper)
        {
            _context = context;
            _solicoWebService = solicoWebService;
            _mapper = mapper;
        }

        #region Service

        public async Task SyncCustomerAsync()
        {
            var solicoCustomerIds = await _context.Customers
                .Select(x => new SolicoPricingDto
                {
                    KUNNR = x.SolicoCustomerId
                }).ToListAsync();

            var solicoCustomers = await _solicoWebService.GetCustomerAsync(solicoCustomerIds, CancellationToken.None);

            List<UpdateSolicoCustomerDto> customers = _mapper.Map<List<UpdateSolicoCustomerDto>>(solicoCustomers);

            if (customers.Any())
            {
                foreach (var solicoCustomer in customers)
                {
                    var customer =
                        await _context.Customers.FirstOrDefaultAsync(x => x.SolicoCustomerId == solicoCustomer.KUNNR);

                    if (customer != null)
                    {
                        _mapper.Map(solicoCustomer, customer);
                    }
                }

                await _context.SaveAsync(CancellationToken.None);
            }
        }

        public async Task SyncMaterialAsync()
        {
            var productList = await _context.Products.ToListAsync();

            var solicoMaterials = await _solicoWebService.GetMaterialListServiceAsync(CancellationToken.None);

            var products = _mapper.Map<List<Product>>(solicoMaterials);


            if (productList.Any())
            {
                products.ForEach(product =>
                {
                    var existProduct = productList.FirstOrDefault(x => x.MaterialId == product.MaterialId);

                    if (existProduct != null)
                    {
                        product.Image = existProduct.Image;
                        product.ProductCategoryId = existProduct.ProductCategoryId;
                        product.MaterialType = existProduct.MaterialType;
                    }
                });
            }


            await _context.Products.BulkMergeAsync(products, opt => opt.ColumnPrimaryKeyExpression = c => c.MaterialId);
        }

        public async Task SyncNewCustomer()
        {
            var newCustomers = await _context.CustomerEnrollments.Where(x => x.IsDone == false).ToListAsync();

            foreach (var customer in newCustomers)
            {
                var solicoCustomers = await _solicoWebService.GetCustomerAsync(new List<SolicoPricingDto>
                {
                    new() {KUNNR = customer.SolicoCustomerId}
                }, CancellationToken.None);


                if (solicoCustomers.Any() == false) continue;


                var customers = _mapper.Map<List<Customer>>(solicoCustomers);
                await _context.Customers.AddRangeAsync(customers);

                customer.IsDone = true;
            }

            await _context.SaveAsync(CancellationToken.None);
        }

        public async Task SyncCustomerPriceAsync()
        {
            var customers = await _context.Customers.ToListAsync();

            foreach (var customer in customers)
            {
                var customerList = _solicoWebService.GetListServiceAsync(new List<SolicoPricingDto>
                {
                    new() {KUNNR = customer.SolicoCustomerId}
                }, CancellationToken.None);

                var customerPrices =  _solicoWebService.GetPriceServiceAsync(new List<SolicoPricingDto>
                {
                    new() {KUNNR = customer.SolicoCustomerId}
                }, CancellationToken.None);

                await Task.WhenAll(customerList, customerPrices);

                var customerListPrice = await customerList;
                var customerProductPriceList = await customerPrices;

                customerListPrice.ForEach(pro =>
                {
                    var existProduct = customerProductPriceList.FirstOrDefault(x => x.MATNR == pro.MATNR);

                    if (existProduct != null)
                    {
                        pro.GROSS_PRICE = existProduct.GROSS_PRICE;
                    }
                });


                var products = await _context.Products
                    .Where(x => customerListPrice.Select(cl => cl.MATNR).Contains(x.MaterialId)).ToListAsync();
                
                
                List<CustomerProductPrice> productPrice = new List<CustomerProductPrice>();
                try
                {
                    foreach (var  productInList in customerListPrice)
                    {
                        var product =  products.FirstOrDefault(x => x.MaterialId == productInList.MATNR);
                     
                        productPrice.Add(new CustomerProductPrice
                        {
                            SyncDate = DateTime.Now,
                            Price = productInList.GROSS_PRICE,
                            CustomerId = customer.Id,
                            ProductId = product.Id
                        });
                    }
                    
                  
                    var customerProductPrice = await _context.CustomerProductPrices
                        .Where(x => x.CustomerId == customer.Id).ToListAsync();

                     _context.CustomerProductPrices.BulkDelete(customerProductPrice);
                    
                    await _context.CustomerProductPrices.BulkInsertAsync(productPrice);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message, e.StackTrace);
                }


               
            }
        }

        public async Task TrackSailedOrderAsync()
        {
            var orders = await GetPendingOrderListAsync();

            foreach (var order in orders)
            {
                try
                {
                    var sailedOrder = await _solicoWebService.GetQuotationSalesOrderServiceAsync(
                        new List<SolicoOrderNumberDto> {new() {VBELN = order.QuotationNumber}},
                        CancellationToken.None);

                    if (sailedOrder.Any() == false) continue;


                    var orderDetails = sailedOrder.FirstOrDefault()!.ORDER.FirstOrDefault()!.ORDER_DETAILS;

                    order.OrderStatus = (int) OrderStatus.Approved;

                    order.FinalAmount = orderDetails.Sum(x => x.NETWR);

                    order.DiscountPrice = orderDetails.Sum(x => x.DISCOUNT);

                    order.Tax = orderDetails.Sum(x => x.TAX + x.LEVY);

                    await AddOrderItemHistoryAsync(order, OrderStatus.Pending, _mapper);

                    var customer = await GetCustomerAsync(sailedOrder);

                    await UpdateOrderItemAsync(orderDetails, order, customer);


                    await _context.SaveAsync(CancellationToken.None);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message, e.StackTrace);
                }
            }
        }

        public async Task SyncCustomerList()
        {
            var customers = await _context.Customers.ToListAsync();

            foreach (var customer in customers)
            {
                var customerPrices = await _solicoWebService.GetListServiceAsync(new List<SolicoPricingDto>
                {
                    new() {KUNNR = customer.SolicoCustomerId}
                }, CancellationToken.None);

                try
                {
                    foreach (var customerPrice in customerPrices)
                    {
                        var customerProductPrice = await HasCustomerPriceExistAsync(customerPrice);

                        if (customerProductPrice is null)
                        {
                            var product = await GetProductAsync(customerPrice);

                            await _context.CustomerProductPrices.AddAsync(new CustomerProductPrice
                            {
                                SyncDate = DateTime.Now,
                                CustomerId = customer.Id,
                                Price = 0,
                                ProductId = product.Id
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message, e.StackTrace);
                }


                await _context.SaveAsync(CancellationToken.None);
            }
        }


        public async Task TrackProformaAsync()
        {
            try
            {
                var orders = await GetApprovedOrderListAsync();


                foreach (var order in orders)
                {
                    var proformaOrder = await _solicoWebService.GetSolicoQuotationProformaServiceAsync(
                        new List<SolicoOrderNumberDto> {new() {VBELN = order.QuotationNumber}}, CancellationToken.None);

                    if (proformaOrder.Any() == false) continue;

                    var orderDetails = proformaOrder.FirstOrDefault()!.PROFORMAS;

                    order.OrderStatus = (int) OrderStatus.Posted;

                    double finalAmount = 0;
                    double discount = 0;
                    double tax = 0;

                    orderDetails.ForEach(detail =>
                    {
                        finalAmount += detail.PROFORMA_DETAILS.Sum(x => x.NETWR);
                        discount += detail.PROFORMA_DETAILS.Sum(x => x.DISCOUNT);
                        tax += detail.PROFORMA_DETAILS.Sum(x => x.TAX + x.LEVY);
                    });

                    order.FinalAmount = finalAmount;
                    order.DiscountPrice = discount;
                    order.Tax = tax;

                    await AddOrderItemHistoryAsync(order, OrderStatus.Approved, _mapper);

                    var customer = await _context.Customers.FirstOrDefaultAsync(x =>
                        x.SolicoCustomerId == proformaOrder.FirstOrDefault().PROFORMAS.FirstOrDefault().KUNNR);

                    int proformaCount = 1;

                    await UpdateOrderItemProformaAsync(orderDetails, order, customer, proformaCount);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);
            }

            await _context.SaveAsync(CancellationToken.None);
        }


        public async Task SyncCreditAsync()
        {
            var customers = await _context.Customers.ToListAsync();

            foreach (var customer in customers)
            {
                var customerCredit = await _solicoWebService.GetCreditServiceAsync(
                    new List<SolicoPricingDto> {new() {KUNNR = customer.SolicoCustomerId}},
                    CancellationToken.None);

                customer.CreditUsed = customerCredit.IT_CREDIT_EXPOSURE.FirstOrDefault()?.USED;
                customer.CreditLimit = customerCredit.IT_CREDIT_EXPOSURE.FirstOrDefault()?.LIMIT;
                customer.CreditExposure = customerCredit.IT_CREDIT_EXPOSURE.FirstOrDefault()?.EXPOSURE;
            }

            await _context.SaveAsync(CancellationToken.None);
        }

        public async Task SyncQuotation()
        {
            var orders = await GetNotSubmitOrdersAsync();


            foreach (var order in orders)
            {
                var token = await _solicoWebService.FetchTokenAsync(CancellationToken.None);

                var quotationItems = AddQuotationItem(order.OrderItems.ToList());

                var response = await _solicoWebService.CreateQuotation(new SolicoQuotationDto
                {
                    I_KUNNR = order.SolicoCustomerId,
                    QUOTATION_ITEMS = quotationItems
                }, token, CancellationToken.None);


                bool success = response.E_RETURN.FirstOrDefault()?.TYPE.ToLower() == "s";
                order.IsSuccess = success;
                order.QuotationNumber = success
                    ? response.E_RETURN.FirstOrDefault()?.MESSAGE.Replace(ResponseMessage.SuccessQuotation, "")
                    : "";

                order.Description = JsonSerializer.Serialize(response.E_RETURN);
            }

            await _context.SaveAsync(CancellationToken.None);
        }

        public async Task SyncOpenItems()
        {
            var customers = await _context.Customers.ToListAsync();
            var tBurk = new List<TBurk> {new() {BUKRS = "1100"}};


            foreach (var customer in customers)
            {
                var openItems = await _solicoWebService.GetOpenItemServiceAsync(new CreateOpenItemDto
                {
                    T_BUKRS = tBurk,
                    T_CUSTOMER_NO = new List<TCustomerNO>
                    {
                        new() {KUNNR = customer.SolicoCustomerId}
                    }
                }, CancellationToken.None);


                var customerOpenItems = _mapper.Map<List<CustomerOpenItem>>(openItems.T_OPEN_ITEMS);

                if (customerOpenItems.Any())
                {
                    customerOpenItems.ForEach(item => { item.CustomerId = customer.Id; });

                    foreach (var items in customerOpenItems)
                    {
                        var document =
                            await _context.CustomerOpenItems.FirstOrDefaultAsync(x =>
                                x.DocumentNumber == items.DocumentNumber);

                        if (document is null)
                        {
                            await _context.CustomerOpenItems.AddAsync(new CustomerOpenItem
                            {
                                DocumentNumber = items.DocumentNumber,
                                Amount = items.Amount,
                                DueDate = items.DueDate,
                                CustomerId = customer.Id,
                                IsPaid = false,
                            });
                        }
                        else
                        {
                            document.Amount = items.Amount;
                            document.DueDate = items.DueDate;
                        }
                    }

                    await _context.SaveAsync(CancellationToken.None);
                }
            }
        }

        public async Task SyncDeliverOrderAsync()
        {
            var orders = await _context.Orders
                .Include(x => x.OrderItems).ThenInclude(x => x.Product)
                .Where(x => x.OrderStatus == (int) OrderStatus.Posted).ToListAsync();

            foreach (var order in orders)
            {
                try
                {
                    var documents = await _solicoWebService.GetDocumentFlowServiceAsync(
                        new List<SolicoOrderNumberDto> {new() {VBELN = order.QuotationNumber}},
                        CancellationToken.None);

                    if (documents.FirstOrDefault()!.INVOICE.Any())
                    {
                        order.OrderStatus = (int) OrderStatus.Delivered;
                        await AddOrderItemHistoryAsync(order, OrderStatus.Posted, _mapper);
                    }

                    await _context.SaveAsync(CancellationToken.None);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message, e.StackTrace);
                }
            }
        }

        public async Task DeleteExpiredUserTokenAsync()
        {
            var userTokens =
                await _context.UserTokens.Where(x => x.IsUsed || x.ExpireDate < DateTime.Now).ToListAsync();

            _context.UserTokens.RemoveRange(userTokens);

            await _context.SaveAsync(CancellationToken.None);
        }

        #endregion

        #region Query

        private async Task UpdateOrderItemProformaAsync(List<ProformaDto> orderDetails, Order order, Customer customer,
            int proformaCount)
        {
            foreach (var solicoProformas in orderDetails.Select(x => x.PROFORMA_DETAILS).ToList())
            {
                var productMaterialIds = order.OrderItems.Select(x => x.Product.MaterialId)
                    .Except(solicoProformas.Select(x => x.MATNR))
                    .ToList();

                foreach (var productMaterialId in productMaterialIds)
                {
                    var removeItem = order.OrderItems.FirstOrDefault(x => x.Product.MaterialId == productMaterialId);

                    if (removeItem != null)
                    {
                        _context.OrderItems.Remove(removeItem);
                    }
                }

                foreach (var items in solicoProformas)
                {
                    var existItem = order.OrderItems.FirstOrDefault(x => x.Product.MaterialId == items.MATNR);

                    if (existItem is null)
                    {
                        var product = await GetProductAsync(items.MATNR);

                        if (product != null)
                        {
                            await AddOrderItemAsync(order, items, product, customer);
                        }
                    }
                    else
                    {
                        UpdateOrderItem(proformaCount, existItem, items);
                    }
                }

                proformaCount++;
            }
        }

        private async Task UpdateOrderItemAsync(List<SolicoOrderDetailDto> orderDetails, Order order, Customer customer)
        {
            var productMaterialIds = order.OrderItems.Select(x => x.Product.MaterialId)
                .Except(orderDetails.Select(x => x.MATNR))
                .ToList();


            foreach (var productMaterialId in productMaterialIds)
            {
                var removeItem = order.OrderItems.FirstOrDefault(x => x.Product.MaterialId == productMaterialId);

                if (removeItem != null)
                {
                    _context.OrderItems.Remove(removeItem);
                }
            }


            foreach (var items in orderDetails)
            {
                var existItem = order.OrderItems.FirstOrDefault(x => x.Product.MaterialId == items.MATNR);
                if (existItem is null)
                {
                    var product = await GetProductAsync(items.MATNR);

                    if (product != null)
                    {
                        await _context.OrderItems.AddAsync(new OrderItem
                        {
                            OrderId = order.Id,
                            CreateDate = DateTime.Now,
                            Count = (int) items.KWMENG,
                            ProductId = product.Id,
                            Price = items.NET_PRICE,
                            CustomerId = customer.Id
                        });
                    }
                }

                else
                {
                    existItem.Count = items.POSNR;
                    existItem.Price = items.NET_PRICE;
                }
            }
        }

        private async Task<Customer> GetCustomerAsync(List<QuotationSalesOrderDto> sailedOrder)
        {
            return await _context.Customers.FirstOrDefaultAsync(x =>
                x.SolicoCustomerId == sailedOrder.FirstOrDefault().ORDER.FirstOrDefault().KUNNR);
        }

        private async Task AddOrderItemHistoryAsync(Order order, OrderStatus status, IMapper mapper)
        {
            await _context.OrderStatusHistories.AddAsync(new OrderStatusHistory
            {
                CreateDate = DateTime.Now,
                OrderStatus = (int) status,
                OrderId = order.Id,
                OrderItems = mapper.Map<List<HistoryOrderItem>>(order.OrderItems.ToList())
            });
        }

        private static void UpdateOrderItem(int proformaCount, OrderItem existItem, SolicoProformaDetailDto items)
        {
            if (proformaCount > 1)
            {
                existItem.Count += items.POSNR;
                existItem.Price += items.NET_PRICE;
            }
            else
            {
                existItem.Count = items.POSNR;
                existItem.Price = items.NET_PRICE;
            }
        }

        private async Task AddOrderItemAsync(Order order, SolicoProformaDetailDto items, Product product,
            Customer customer)
        {
            await _context.OrderItems.AddAsync(new OrderItem
            {
                OrderId = order.Id,
                CreateDate = DateTime.Now,
                Count = (int) items.FKIMG,
                ProductId = product.Id,
                Price = items.NET_PRICE,
                CustomerId = customer.Id
            });
        }

        private async Task<List<Order>> GetNotSubmitOrdersAsync()
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Where(x => x.IsSuccess == false && x.OrderStatus != (int) OrderStatus.Canceled &&
                            string.IsNullOrWhiteSpace(x.Description)).ToListAsync();
        }

        private async Task<List<Order>> GetApprovedOrderListAsync()
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Where(x => x.OrderStatus == (int) OrderStatus.Approved)
                .ToListAsync();
        }

        private async Task<Product> GetProductAsync(SolicoListingServiceDto customerPrice)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.MaterialId == customerPrice.MATNR);
        }

        private async Task<CustomerProductPrice> HasCustomerPriceExistAsync(SolicoListingServiceDto customerPrice)
        {
            return await _context.CustomerProductPrices
                .SingleOrDefaultAsync(x =>
                    x.Customer.SolicoCustomerId == customerPrice.KUNNR &&
                    x.Product.MaterialId == customerPrice.MATNR);
        }

        private async Task<Product> GetProductAsync(string matnr)
            => await _context.Products.FirstOrDefaultAsync(x => x.MaterialId == matnr);

        private async Task<List<Order>> GetPendingOrderListAsync()
            => await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .Where(x => x.OrderStatus == (int) OrderStatus.Pending && !string.IsNullOrWhiteSpace(x.QuotationNumber))
                .ToListAsync();


        private static List<QuotationItem> AddQuotationItem(List<OrderItem> orderItems)
        {
            List<QuotationItem> quotationItems = new List<QuotationItem>();

            int count = 1;
            foreach (var item in orderItems)
            {
                quotationItems.Add(new QuotationItem
                {
                    ITM_NUMBER = $"0000{count * 10}",
                    MATERIAL = item.Product.MaterialId,
                    TARGET_QU = item.Product.Unit,
                    TARGET_QTY = item.Count.ToString()
                });
                count++;
            }

            return quotationItems;
        }

        #endregion
    }
}