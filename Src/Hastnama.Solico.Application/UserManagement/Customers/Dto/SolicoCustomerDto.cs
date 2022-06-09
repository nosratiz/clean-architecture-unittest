namespace Hastnama.Solico.Application.UserManagement.Customers.Dto
{
    public class SolicoCustomerDto
    {
        //Customer Number
        public string KUNNR { get; set; }
    
        //Country Key
        public string LAND1 { get; set; }
       //region
        public string REGIO { get; set; }
        //city
        public string CITYC { get; set; }
      
        public string ADRNR { get; set; }
        //name
        public string NAME1 { get; set; }
        public string NAME2 { get; set; }
        public string NAME3 { get; set; }
        public string NAME4 { get; set; }
        //phone
        public string ZZNOTE_1 { get; set; }
        //mobile
        public string ZZNOTE_2 { get; set; }
       
        public string ADDRNUMBER { get; set; }
       //address
        public string STR_SUPPL1 { get; set; }
        public string STR_SUPPL2 { get; set; }
        public string STR_SUPPL3 { get; set; }
        public string LOCATION { get; set; }
        public string KUNN2 { get; set; }
    }
    
    public class UpdateSolicoCustomerDto
    {
        //Customer Number
        public string KUNNR { get; set; }
    
        //Country Key
        public string LAND1 { get; set; }
        //region
        public string REGIO { get; set; }
        //city
        public string CITYC { get; set; }
      
        public string ADRNR { get; set; }
        //name
        public string NAME1 { get; set; }
        public string NAME2 { get; set; }
        public string NAME3 { get; set; }
        public string NAME4 { get; set; }
        //phone
        public string ZZNOTE_1 { get; set; }
        //mobile
        public string ZZNOTE_2 { get; set; }
       
        public string ADDRNUMBER { get; set; }
        //address
        public string STR_SUPPL1 { get; set; }
        public string STR_SUPPL2 { get; set; }
        public string STR_SUPPL3 { get; set; }
        public string LOCATION { get; set; }
        public string KUNN2 { get; set; }
    }
}