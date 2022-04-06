using System.Text.Json;

namespace CSharpNotion.Extensions
{
    internal static class HttpExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        public static async Task<HttpResponseMessage> Send(this HttpRequestMessage request, HttpClient httpClient) => await httpClient.SendAsync(request);

        public static async Task<T> DeserializeJson<T>(this Task<HttpResponseMessage> responseTask)
        {
            HttpResponseMessage response = await responseTask;
            return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStreamAsync(), Constants.SerializeOptions)!;
        }
    }
}