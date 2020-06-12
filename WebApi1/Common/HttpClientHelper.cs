using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi1.Common
{
    public class HttpClientHelper
    {
        private readonly IHttpClientFactory _clientFactory;
        public HttpClientHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> GetAsync(string url)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var response = await client.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        public async Task<string> PostAsync(string url, IDictionary<string, object> dic, string token)
        {
            using (var client = _clientFactory.CreateClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(dic == null ? "" : JsonConvert.SerializeObject(dic)),
                    RequestUri = new Uri(url)
                };

                requestMessage.Headers.Add("Authorization", "Bearer " + token);

                var response = await client.SendAsync(requestMessage);

                var contents = await response.Content.ReadAsStringAsync();

                return contents;
            }
        }
    }
}
