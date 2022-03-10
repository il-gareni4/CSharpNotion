using System.Text.Json;

namespace CSharpNotion
{
    internal static class HttpExtension
    {
        public static async Task<HttpResponseMessage> Send(this HttpRequestMessage request, HttpClient httpClient) => await httpClient.SendAsync(request);

        public static async Task<T> DeserializeJson<T>(this Task<HttpResponseMessage> responseTask)
        {
            HttpResponseMessage response = await responseTask;
            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStreamAsync(), Client.SerializeOptions)!;
        }
    }
}
