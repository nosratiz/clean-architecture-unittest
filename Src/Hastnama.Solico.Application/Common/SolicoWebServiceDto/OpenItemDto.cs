using System.Collections.Generic;

namespace Hastnama.Solico.Application.Common.SolicoWebServiceDto
{
    public class SolicoOpenItemDto
    {
        public Ereturn E_RETURN { get; set; }
        public List<openItem> T_OPEN_ITEMS { get; set; }
    }
    
    
    public class openItem
    {
        //Company Code
        public string BUKRS { get; set; }
        //Customer
        public string KUNNR { get; set; }
        //Special G/L ind
        public string UMSKZ { get; set; }
        //Document Number
        public string BELNR { get; set; }
        //Debit/Credit Ind.
        public string SHKZG { get; set; }
        //Amount in LC
        public double DMBTR { get; set; }
       //Baseline Payment Date
        public string ZFBDT { get; set; }
        //Terms of Payment
        public string ZTERM { get; set; }
        //Days 1
        public int ZBD1T { get; set; }
        //Credit control area
        public string KKBER { get; set; }
        //Collection day
        public string KRAUS { get; set; }
    }

}