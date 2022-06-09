using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace Hastnama.Solico.Application.Common.Interfaces
{
    public interface IHttpStrategy
    {
        Task<IRestResponse> ExecuteRequest(string url, string body = "",IDictionary<string,string> headers=null);
    }
}