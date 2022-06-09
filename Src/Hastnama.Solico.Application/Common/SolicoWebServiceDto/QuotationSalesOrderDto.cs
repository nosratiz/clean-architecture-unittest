using System.Collections.Generic;

namespace Hastnama.Solico.Application.Common.SolicoWebServiceDto
{
    public class QuotationSalesOrderDto
    {
        public string QUOTATION { get; set; }
        public List<SolicoOrderDto> ORDER { get; set; }
    }
    
    
    public class SolicoOrderDetailDto
    {
        //Order No.
        public string VBELN { get; set; }
        //Order Line Number.
        public int POSNR { get; set; }
        //Material Number
        public string MATNR { get; set; }
        //Order Qty
        public double KWMENG { get; set; }
       //Sales Unit
        public string VRKME { get; set; }
        //Net Price of Material
        public double NET_PRICE { get; set; }
        //Discount
        public double DISCOUNT { get; set; }
        
        //Tax
        public double TAX { get; set; }
        
        //Levy
        public double LEVY { get; set; }
        
        //Net Value of one line of Order
        public double NETWR { get; set; }
        
        //Currrency
        public string WAERK { get; set; }
    }

    public class SolicoOrderDto
    {
        //Quotation Number
        public string VBELV { get; set; }
        //Order Number
        public string VBELN { get; set; }
       //Order Date
        public string AUDAT { get; set; }
        
        //Currency
        public string WAERK { get; set; }
        
        //Customer Number
        public string KUNNR { get; set; }
        
        //Payer Number
        public string PAYER { get; set; }
        
        public List<SolicoOrderDetailDto> ORDER_DETAILS { get; set; }
    }

    public class CreateOpenItemDto
    {
        public List<TBurk> T_BUKRS { get; set; }
        public List<TCustomerNO> T_CUSTOMER_NO { get; set; }
    }
    
    public class TBurk
    {
        public string BUKRS { get; set; }
    }

    public class TCustomerNO
    {
        public string KUNNR { get; set; }
    }

    


}