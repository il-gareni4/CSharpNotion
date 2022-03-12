using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class FileBlock : FileContainingBlock
    {
        public FileBlock(RecordMapBlockValue blockValue) : base(blockValue)
        { }

        public override async Task SetFileUrl(string source)
        {
            if (source == Source) return;
            try
            {
                Dictionary<string, object?> propertiesArgs = new() { { "source", new string[][] { new string[] { source } } } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, propertiesArgs);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Source = source;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}