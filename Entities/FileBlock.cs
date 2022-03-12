using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class FileBlock : FileContainingBlock
    {
        public FileBlock(RecordMapBlockValue blockValue) : base(blockValue)
        { }

        public async Task SetFileUrl(string fileUrl) => await base.BaseSetFileUrl(fileUrl, false);

        public async Task UploadAndSetFile(string filePath, string? fileName = null)
        {
            GetUploadFileUrlResponse urlsResponse = await Api.ApiMaster.UploadFile(Client.HttpClient, Id, filePath);
            await SetFileUrl(urlsResponse.Url!);
            fileName ??= new FileInfo(filePath).Name;
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { fileName } } } };
            Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args);
            (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
        }
    }
}