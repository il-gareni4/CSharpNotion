using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class FileBlock : FileContainingBlock, IColorBlock
    {
        public BlockColor Color { get; set; }

        public FileBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

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

        public async Task SetColor(BlockColor color)
        {
            if (color == Color) return;
            try
            {
                Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Color = color;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}