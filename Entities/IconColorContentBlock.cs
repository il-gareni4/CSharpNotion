namespace CSharpNotion.Entities
{
    public abstract class IconColorContentBlock : TitleContentBlock, IColorBlock
    {
        public string? Icon { get; protected set; }
        public BlockColor Color { get; set; }

        public IconColorContentBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Icon = blockValue?.Format?.PageIcon ?? null;
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

        public virtual async Task SetIcon(string? icon)
        {
            if (Icon == icon) return;
            try
            {
                Dictionary<string, object?> args = new() { { "page_icon", icon } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Icon = icon;
            }
            catch (Exception ex)
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