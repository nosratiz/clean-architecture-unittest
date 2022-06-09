using System.Collections.Generic;

namespace Hastnama.Solico.Application.Common.SolicoWebServiceDto
{
    public class SolicoQuotationDto
    {
        //Customer Number
        public string I_KUNNR { get; set; }
        public List<QuotationItem> QUOTATION_ITEMS { get; set; }
    }

    public class QuotationItem
    {
        //Quotation Number
        public string ITM_NUMBER { get; set; }

        //Material Number
        public string MATERIAL { get; set; }

        //Target Qty in Sales Units
        public string TARGET_QTY { get; set; }
      
        //Sales UoM of the material
        public string TARGET_QU { get; set; }
    }
}