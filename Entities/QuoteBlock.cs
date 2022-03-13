namespace CSharpNotion.Entities
{
    public enum FormatQuoteSize
    {
        default_size,
        large
    }

    public class QuoteBlock : ColorTitleContentBlock
    {
        public FormatQuoteSize QuoteSize { get; private set; }

        public QuoteBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            QuoteSize = blockValue?.Format?.QuoteSize switch
            {
                "large" => FormatQuoteSize.large,
                _ => FormatQuoteSize.default_size
            };
        }

        public async Task SetQuoteSize(FormatQuoteSize size)
        {
            if (size == QuoteSize) return;
            try
            {
                Dictionary<string, object?> args = new() { { "quote_size", size == FormatQuoteSize.default_size ? null : "large" } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                QuoteSize = size;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}