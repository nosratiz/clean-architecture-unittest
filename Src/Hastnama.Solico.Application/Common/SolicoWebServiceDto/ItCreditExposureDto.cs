using System.Collections.Generic;

namespace Hastnama.Solico.Application.Common.SolicoWebServiceDto
{
    public class ItCreditExposureDto
    {
        //Customer Number
        public string KUNNR { get; set; }
      
        //Payer (same as KUNNR)
        public string PAYER { get; set; }
        
        //Credit limit
        public double LIMIT { get; set; }
        //Credit Exposure
        public double EXPOSURE { get; set; }
       
        //Credit limit Used
        public double USED { get; set; }
        
        //Currency
        public string CMWAE { get; set; }
    }

    public class CreditDto
    {
        public List<ItCreditExposureDto> IT_CREDIT_EXPOSURE { get; set; }
        public List<object> IT_OPEN_ITEMS { get; set; }
    }
}