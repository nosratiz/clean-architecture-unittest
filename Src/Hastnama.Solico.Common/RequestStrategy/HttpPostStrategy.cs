using System.Collections.Generic;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using RestSharp;

namespace Hastnama.Solico.Application.Common.Services
{
    public class HttpPostStrategy : IHttpStrategy
    {
        public async Task<IRestResponse> ExecuteRequest(string url, string body = "{}", IDictionary<string, string> headers = null)
        {
            var client = new RestClient(url);

            var request = new RestRequest {Method = Method.POST};


            request.AddParameter("application/json", body, ParameterType.RequestBody);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            return await client.ExecuteAsync(request);
        }
    }
}