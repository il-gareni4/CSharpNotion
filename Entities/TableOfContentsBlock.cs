namespace CSharpNotion.Entities
{
    public class TableOfContentsBlock : BaseBlock, IColorBlock
    {
        public BlockColor Color { get; set; }

        public TableOfContentsBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        {
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
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