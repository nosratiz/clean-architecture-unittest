namespace Hastnama.Solico.Application.Shop.Products.Dto
{
    public class SolicoMaterialDto
    {
        //material Number
        public string MATNR { get; set; }

        //material type
        public string MTART { get; set; }
        
        //division
        public string SPART { get; set; }
        
        //material group
        public string MATKL { get; set; }
       
        //base Uof measure
        public string MEINS { get; set; }
        
        //Sale UoF Measure
        public string VRKME { get; set; }
        
        //Material Description
        public string MAKTX { get; set; }
        
        //Material Group Description
        public string WGBEZ { get; set; }
        
        //Long Mat. Group Desc.
        public string WGBEZ60 { get; set; }
        
        //Division Description
        public string VTEXT { get; set; }
    }
}