using System.Net.Http.Headers;
using System.Text.Json;

namespace CSharpNotion.Utilities
{
    internal class HttpUtils
    {
        private HttpUtils()
        { }

        public static void SetHttpContent(ref HttpRequestMessage httpRequest, object data)
        {
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(data, Constants.SerializeOptions));
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}