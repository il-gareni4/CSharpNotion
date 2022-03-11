namespace CSharpNotion.Entities
{
    public abstract class IconContentBlock : TitleContentBlock
    {
        public string? Icon { get; protected set; }

        public IconContentBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        {
            Icon = blockValue?.Format?.PageIcon ?? null;
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
    }
}