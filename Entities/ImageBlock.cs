using CSharpNotion.Api.Response;
using System.Net.Http.Headers;

namespace CSharpNotion.Entities
{
    public class ImageBlock : BaseBlock, ICaptionBlock
    {
        public string Caption { get; protected set; }
        public string? Source { get; protected set; }
        public string? DisplaySource { get; protected set; }

        public ImageBlock(RecordMapBlockValue blockValue) : base(blockValue)
        {
            Caption = blockValue?.Properties?.Caption?.ElementAt(0)[0].GetString() ?? "";
            Source = blockValue?.Properties?.Source?.ElementAt(0)[0];
            DisplaySource = blockValue?.Format?.DisplaySource;
        }

        public async Task SetCaption(string caption)
        {
            if (caption == Caption) return;
            try
            {
                Dictionary<string, object?> args = new() { { "caption", new string[][] { new string[] { caption } } } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Caption = caption;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public async Task SetImageUrl(string? imageUrl)
        {
            if (imageUrl == Source) return;
            try
            {
                Dictionary<string, object?> propertiesArgs = new() { { "source", new string?[][] { new string?[] { imageUrl } } } };
                Dictionary<string, object?> formatArgs = new() { { "display_source", imageUrl } };
                Api.Request.Operation[] operations = new Api.Request.Operation[] {
                    Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, propertiesArgs),
                    Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, formatArgs)
                };
                (await QuickRequestSetup.SaveTransactions(operations).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Source = imageUrl;
                DisplaySource = imageUrl;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public async Task UploadAndSetImage(string imagePath)
        {
            try
            {
                FileInfo imageInfo = new(imagePath);
                GetUploadFileUrlResponse urlsResponse = await QuickRequestSetup.GetUploadFileUrl(SelfPointer, imageInfo)
                                                                               .Send(Client.HttpClient)
                                                                               .DeserializeJson<GetUploadFileUrlResponse>();
                HttpRequestMessage imageRequest = new(HttpMethod.Put, urlsResponse.SignedPutUrl);
                imageRequest.Content = new ByteArrayContent(File.ReadAllBytes(imagePath));
                imageRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypes.MimeTypeMap.GetMimeType(imageInfo.Extension));
                (await Client.HttpClient.SendAsync(imageRequest)).EnsureSuccessStatusCode();

                await SetImageUrl(urlsResponse.Url);
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}