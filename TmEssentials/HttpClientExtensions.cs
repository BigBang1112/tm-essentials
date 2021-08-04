using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

#if NETSTANDARD2_0

using System.Net.Http;

#endif

namespace TmEssentials
{
    public static class HttpClientExtensions
    {

#if NETSTANDARD2_0

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="http">HttpClient.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HeadAsync(this HttpClient http, string requestUri)
        {
            var head = new HttpRequestMessage(HttpMethod.Head, requestUri);
            return await http.SendAsync(head);
        }

#endif

    }
}
