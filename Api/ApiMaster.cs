using CSharpNotion.Api.General;
using CSharpNotion.Api.Response;
using System.Net.Http.Headers;

namespace CSharpNotion.Api
{
    internal static class ApiMaster
    {
        public static async Task<GetUploadFileUrlResponse> UploadFile(HttpClient httpClient, string blockId, string filePath)
        {
            FileInfo fileInfo = new(filePath);
            GetUploadFileUrlResponse urlsResponse = await QuickRequestSetup.GetUploadFileUrl(new Pointer(blockId, "block"), fileInfo)
                                                                           .Send(httpClient)
                                                                           .DeserializeJson<GetUploadFileUrlResponse>();
            if (urlsResponse.Url is null) throw new InvalidDataException($"Cannot get upload URL for this file: {filePath}");
            HttpRequestMessage imageRequest = new(HttpMethod.Put, urlsResponse.SignedPutUrl);
            imageRequest.Content = new ByteArrayContent(File.ReadAllBytes(filePath));
            imageRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypes.MimeTypeMap.GetMimeType(fileInfo.Extension));
            (await Client.HttpClient.SendAsync(imageRequest)).EnsureSuccessStatusCode();
            return urlsResponse;
        }
    }
}
