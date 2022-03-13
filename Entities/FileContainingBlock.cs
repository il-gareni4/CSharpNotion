using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public abstract class FileContainingBlock<T> : BaseBlock, ICaptionBlock<T> where T : BaseBlock
    {
        public string Caption { get; protected set; }
        public string? Source { get; protected set; }
        public string? DisplaySource { get; protected set; }
        public string[]? FileIds { get; protected set; }

        public FileContainingBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Caption = blockValue?.Properties?.Caption?.ElementAt(0)[0].GetString() ?? "";
            Source = blockValue?.Properties?.Source?.ElementAt(0)[0];
            DisplaySource = blockValue?.Format?.DisplaySource;
            FileIds = blockValue?.FileIds;
        }

        public abstract T SetCaption(string caption);

        public abstract T SetFileUrl(string source);

        //{
        //    if (source == Source) return;
        //    try
        //    {
        //        Dictionary<string, object?> propertiesArgs = new() { { "source", new string[][] { new string[] { source } } } };
        //        Dictionary<string, object?> formatArgs = new() { { "display_source", source } };
        //        List<Api.Request.Operation> operations = new() {
        //            Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, propertiesArgs),
        //            Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, formatArgs)
        //        };
        //        (await QuickRequestSetup.SaveTransactions(operations.ToArray()).Send(Client.HttpClient)).EnsureSuccessStatusCode();
        //        Source = source;
        //        DisplaySource = source;
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        Console.Error.WriteLine(ex.Message);
        //    }
        //}

        public virtual async Task UploadAndSetFileAsync(string filePath, string? fileName = null)
        {
            GetUploadFileUrlResponse urlsResponse = await Api.ApiMaster.UploadFile(Client, Id, filePath);

            Dictionary<string, object?> propertiesArgs = new() { { "source", new string[][] { new string[] { urlsResponse.Url! } } } };
            Dictionary<string, object?> formatArgs = new() { { "display_source", urlsResponse.Url } };
            List<Api.Request.Operation> operations = new()
            {
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, propertiesArgs),
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, formatArgs)
            };
            if (fileName != "")
            {
                fileName ??= new FileInfo(filePath).Name;
                Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { fileName } } } };
                operations.Add(Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args));
            }
            (await QuickRequestSetup.SaveTransactions(operations.ToArray()).Send(Client.HttpClient)).EnsureSuccessStatusCode();
            Source = DisplaySource = urlsResponse.Url;
        }
    }
}