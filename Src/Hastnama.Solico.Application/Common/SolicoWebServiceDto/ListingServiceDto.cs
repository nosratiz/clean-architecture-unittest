namespace Hastnama.Solico.Application.Common.SolicoWebServiceDto
{
    public class SolicoListingServiceDto
    {
        //Customer Number
        public string KUNNR { get; set; }
       
        //Material Number
        public string MATNR { get; set; }
      
        //Division
        public double GROSS_PRICE { get; set; }
        
        //Currency
        public string WAERS { get; set; }
        
        //Condition pricing unit
        public int KPEIN { get; set; }
        
        //Condition unit
        public string KMEIN { get; set; }
    }
}