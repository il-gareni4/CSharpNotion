using CSharpNotion.Api.General;
using CSharpNotion.Api.Response;
using CSharpNotion.Entities.Interfaces;

namespace CSharpNotion.Entities
{
    public abstract class FileContainingBlock<T> : BaseBlock, ICaptionBlock<T> where T : BaseBlock
    {
        public string Caption { get; protected set; }
        public string? Source { get; protected set; }
        public string? DisplaySource { get; protected set; }
        public string[]? FileIds { get; protected set; }

        internal FileContainingBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Caption = blockValue?.Properties?.GetValueOrDefault("caption")?.ElementAt(0)[0].GetString() ?? "";
            Source = blockValue?.Properties?.GetValueOrDefault("source")?.ElementAt(0)[0].GetString();
            DisplaySource = blockValue?.Format?.DisplaySource;
            FileIds = blockValue?.FileIds;
        }

        public abstract T SetCaption(string caption);

        public abstract T SetFileUrl(string source);

        /// <summary>
        /// Uploads a file to the Notion server and sets a link (<see cref="ContentBlock.Content"/>) to the file to the block.
        /// </summary>
        /// <remarks>
        /// If you have just created a new block, you need to <see cref="Client.Commit"/> the inserting action before using this method.
        /// </remarks>
        /// <param name="filePath">Path of the file you need to upload</param>
        /// <param name="fileName">File name that will be displayed on the Notion</param>
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
            (await ReqSetup.SaveTransactions(operations.ToArray()).Send(Client.HttpClient)).EnsureSuccessStatusCode();
            Source = DisplaySource = urlsResponse.Url;
        }
    }
}