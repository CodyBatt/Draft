using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Flurl;
using Flurl.Http;
using Flurl.Http.Content;

namespace Draft
{
    internal static class FlurlExtensions
    {

        public static Url ToUrl(this Uri This)
        {
            return This.ToString();
        }

        public static Url Conditionally(this Url This, bool predicate, Func<Url, Url> action)
        {
            return predicate ? action(This) : This;
        }

        public static Task<HttpResponseMessage> Conditionally(this Url This, bool predicate, object data, Func<Url, object, Task<HttpResponseMessage>> ifTrue, Func<Url, object, Task<HttpResponseMessage>> ifFalse)
        {
            return predicate ? ifTrue(This, data) : ifFalse(This, data);
        }

        /// <summary>
        /// Sends an asynchronous PUT request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="client">The Flurl client.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        private static Task<HttpResponseMessage> PutUrlEncodedAsync(this FlurlClient client, object data)
        {
            var content = new CapturedUrlEncodedContent(client.Settings.UrlEncodedSerializer.Serialize(data));
            return client.SendAsync(HttpMethod.Put, content: content);
        }

        /// <summary>
        /// Creates a FlurlClient from the URL and sends an asynchronous PUT request.
        /// </summary>
        /// <param name="data">Contents of the request body.</param>
        /// <param name="url">The URL.</param>
        /// <returns>A Task whose result is the received HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PutUrlEncodedAsync(this Flurl.Url url, object data)
        {
            return new FlurlClient(url, false).PutUrlEncodedAsync(data);
        }
    }
}
