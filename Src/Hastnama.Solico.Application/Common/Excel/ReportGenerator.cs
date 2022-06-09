using ClosedXML.Excel;
using DNTPersianUtils.Core;
using Hastnama.Solico.Common.Enums;
using Hastnama.Solico.Common.Environment;
using Hastnama.Solico.Common.Helper;
using Hastnama.Solico.Domain.Models.Shop;
using Hastnama.Solico.Domain.Models.UserManagement;
using Serilog;
using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Math;

namespace Hastnama.Solico.Application.Common.Excel
{
    public static class ReportGenerator
    {
        public static string CustomerList(List<Customer> customers)
        {
            try
            {
                using var workbook = new XLWorkbook();

                IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Customers");

                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();

                worksheet.Cell(1, 1).Value = "ردیف";
                worksheet.Cell(1, 2).Value = "نام";
                worksheet.Cell(1, 3).Value = "شماره مشتری";
                worksheet.Cell(1, 4).Value = "شماره تماس 1";
                worksheet.Cell(1, 5).Value = "شماره تماس 2";
                worksheet.Cell(1, 6).Value = "آدرس";
                worksheet.Cell(1, 7).Value = "کد شهر";


                for (int index = 1; index <= customers.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = customers[index - 1].FullName;
                    worksheet.Cell(index + 1, 3).Value = customers[index - 1].SolicoCustomerId;
                    worksheet.Cell(index + 1, 4).Value = customers[index - 1].Mobile;
                    worksheet.Cell(index + 1, 5).Value = customers[index - 1].Phone;
                    worksheet.Cell(index + 1, 6).Value = customers[index - 1].Address;
                    worksheet.Cell(index + 1, 7).Value = customers[index - 1].CityCode;
                }

                var uniqueName = $" لیست پرسنل - {DateTime.Now.ToLongPersianDateString()}.xlsx";
                var fileName = $"{ApplicationStaticPath.Documents}/{uniqueName}";

                workbook.SaveAs(fileName);

                return $"{ApplicationStaticPath.Clients.Document}/{uniqueName}";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex.StackTrace);
                return string.Empty;
            }
        }

        public static string ProductList(List<Product> products)
        {
            try
            {
                using var workbook = new XLWorkbook();

                IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Product");
                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();

                worksheet.Cell(1, 1).Value = "ردیف";
                worksheet.Cell(1, 2).Value = "نام";
                worksheet.Cell(1, 3).Value = "کد محصول";
                worksheet.Cell(1, 4).Value = "دسته بندی محصول";
                worksheet.Cell(1, 5).Value = "واحد محصول";
                worksheet.Cell(1, 6).Value = "توضیحات";
                worksheet.Cell(1, 7).Value = "نوع محصول";


                for (int index = 1; index <= products.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = products[index - 1].Name;
                    worksheet.Cell(index + 1, 3).Value = products[index - 1].MaterialId;
                    worksheet.Cell(index + 1, 4).Value = products[index - 1].ProductCategory?.Name;
                    worksheet.Cell(index + 1, 5).Value = products[index - 1].Unit;
                    worksheet.Cell(index + 1, 6).Value = products[index - 1].Description;
                    worksheet.Cell(index + 1, 7).Value = products[index - 1].MaterialType;
                }

                var uniqueName = $" لیست محصولات - {DateTime.Now.ToLongPersianDateString()}.xlsx";
                var fileName = $"{ApplicationStaticPath.Documents}/{uniqueName}";

                workbook.SaveAs(fileName);

                return $"{ApplicationStaticPath.Clients.Document}/{uniqueName}";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex.StackTrace);
                return string.Empty;
            }
        }

        public static string OrderList(List<Order> orders)
        {
            try
            {
                using var workbook = new XLWorkbook();

                IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Order");
                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();
                worksheet.Columns("B").Width = 30;
                worksheet.Columns("C").Width = 30;
                
                worksheet.Cell(1, 1).Value = "ردیف";
                worksheet.Cell(1, 2).Value = " نام خریدار";
                worksheet.Cell(1, 3).Value = "آدرس خریدار";
                worksheet.Cell(1, 4).Value = "قیمت فاکتور";
                worksheet.Cell(1, 5).Value = "وضعیت سفارش";
                worksheet.Cell(1, 6).Value = "شماره فاکتور سولیکو";
                worksheet.Cell(1, 7).Value = "ثبت موفق در وب سرویس سولیکو";
                worksheet.Cell(1, 8).Value = "تاریخ ثبت ";

                worksheet.Column(4).DataType = XLDataType.Text;
                worksheet.Column(8).DataType = XLDataType.Text;


                for (int index = 1; index <= orders.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = index;
                    worksheet.Cell(index + 1, 2).Value = orders[index - 1].DeliveryName;
                    worksheet.Cell(index + 1, 3).Value = orders[index - 1].Address;
                    worksheet.Cell(index + 1, 4).Value = orders[index - 1].FinalAmount.ToString("##,###");
                    worksheet.Cell(index + 1, 5).Value =
                        EnumConvertor.GetDisplayName((OrderStatus)orders[index - 1].OrderStatus);
                    worksheet.Cell(index + 1, 6).Value = orders[index - 1].QuotationNumber;
                    worksheet.Cell(index + 1, 7).Value = orders[index - 1].IsSuccess ? "بلی" : "خیر";
                    worksheet.Cell(index + 1, 8).Value = orders[index - 1].CreateDate.ToShortPersianDateTimeString();
                }


                var uniqueName = $" لیست سفارشات - {DateTime.Now.ToLongPersianDateString()}.xlsx";
                var fileName = $"{ApplicationStaticPath.Documents}/{uniqueName}";

                workbook.SaveAs(fileName);

                return $"{ApplicationStaticPath.Clients.Document}/{uniqueName}";
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex.StackTrace);
                return string.Empty;
            }
        }
    }
}