using System.Collections.Generic;

namespace Hastnama.Solico.Application.Common.SolicoWebServiceDto
{
    public class DocumentFlowDto
    {
        //Quotation Doc No.
        public string QUOTATION { get; set; }
        public List<SolicoDocumentOrderDto> ORDER { get; set; }
        public List<SolicoProformaDto> PROFORMA { get; set; }
        public List<SolicoInvoiceDto> INVOICE { get; set; }
    }
    
    public class SolicoDocumentOrderDto
    {
        public string VBELN { get; set; }
        public string ERDAT { get; set; }
    }

    public class SolicoProformaDto
    {
        public string VBELN { get; set; }
        public string ERDAT { get; set; }
    }

    public class SolicoInvoiceDto
    {
        public string VBELN { get; set; }
        public string ERDAT { get; set; }
    }
}