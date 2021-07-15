using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TmEssentials
{
    public static class HttpClientExtensions
    {

#if NETSTANDARD2_0

        public static async Task<HttpResponseMessage> HeadAsync(this HttpClient http, string requestUri)
        {
            var head = new HttpRequestMessage(HttpMethod.Head, requestUri);
            return await http.SendAsync(head);
        }

#endif

    }
}
