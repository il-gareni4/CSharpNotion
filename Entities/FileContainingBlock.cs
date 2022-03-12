using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public abstract class FileContainingBlock : BaseBlock, ICaptionBlock
    {
        public string Caption { get; protected set; }
        public string? Source { get; protected set; }
        public string? DisplaySource { get; protected set; }

        public FileContainingBlock(RecordMapBlockValue blockValue) : base(blockValue)
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

        protected virtual async Task SetFileUrl(string source, bool setDisplaySource = true)
        {
            if (source == Source) return;
            try
            {
                Dictionary<string, object?> propertiesArgs = new() { { "source", new string[][] { new string[] { source } } } };
                List<Api.Request.Operation> operations = new() {
                    Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, propertiesArgs),
                };
                if (setDisplaySource)
                {
                    Dictionary<string, object?> formatArgs = new() { { "display_source", source } };
                    operations.Add(Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, formatArgs));
                }
                (await QuickRequestSetup.SaveTransactions(operations.ToArray()).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Source = source;
                if (setDisplaySource) DisplaySource = source;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        protected virtual async Task UploadAndSetFile(string filePath)
        {
            GetUploadFileUrlResponse urlsResponse = await Api.ApiMaster.UploadFile(Client.HttpClient, Id, filePath);
            await SetFileUrl(urlsResponse.Url!);
        }
    }
}
