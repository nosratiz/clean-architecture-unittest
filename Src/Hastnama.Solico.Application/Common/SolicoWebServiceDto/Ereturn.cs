using System.Collections.Generic;

namespace Hastnama.Solico.Application.Common.SolicoWebServiceDto
{
    public class Ereturn
    {
        public string TYPE { get; set; }
        public string MESSAGE { get; set; }
    }

    public class QuotationResponse
    {
        public string SALES_DOCUMENT { get; set; }
        public List<Ereturn> E_RETURN { get; set; }
    }
}