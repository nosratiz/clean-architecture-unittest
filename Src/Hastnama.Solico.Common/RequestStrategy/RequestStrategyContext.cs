using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hastnama.Solico.Application.Common.Interfaces;
using Hastnama.Solico.Application.Common.Services;
using Hastnama.Solico.Common.Enums;
using RestSharp;

namespace Hastnama.Solico.Common.RequestStrategy
{
    public class RequestStrategyContext
    {
        private IHttpStrategy _httpStrategy;


        public IHttpStrategy DetectStrategy(HttpMethod method)
        {
            switch (method)
            {
                case HttpMethod.Get:
                    return _httpStrategy = new HttpGetStrategy();


                case HttpMethod.Post:
                    return _httpStrategy = new HttpPostStrategy();


                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<IRestResponse> ExecuteRequest(string url, string body,
            IDictionary<string, string> headers = null)
        {
            return await _httpStrategy.ExecuteRequest(url, body, headers);
        }
    }
}