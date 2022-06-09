using System.Collections.Generic;

namespace Hastnama.Solico.Application.Common.SolicoWebServiceDto
{
    public class SolicoQuotationProformaDto
    {
        //Quotation Number
        public string QUOTATION { get; set; }
        public List<ProformaDto> PROFORMAS { get; set; }
    }

    public class SolicoProformaDetailDto
    {
        //Proforma No.
        public string VBELN { get; set; }

        //Proforma Line Number.
        public int POSNR { get; set; }

        //Material Number
        public string MATNR { get; set; }

        //Proforma Qty
        public double FKIMG { get; set; }

        //Sales Unit
        public string VRKME { get; set; }

        //Net Price of Material
        public double NET_PRICE { get; set; }

        //Discount
        public int DISCOUNT { get; set; }

        //Tax
        public double TAX { get; set; }

        //Levy
        public double LEVY { get; set; }

        //Net Value of one line of Proforma
        public double NETWR { get; set; }
    }

    public class ProformaDto
    {
        //Quotation Number
        public string VBELV { get; set; }

        //Proforma Number
        public string VBELN { get; set; }

        //Proforma Date
        public string FKDAT { get; set; }

        //Customer Number
        public string KUNNR { get; set; }

        //Payer Number
        public string PAYER { get; set; }

        //Currency
        public string WAERK { get; set; }
        public List<SolicoProformaDetailDto> PROFORMA_DETAILS { get; set; }
    }
}