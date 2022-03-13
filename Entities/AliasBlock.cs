namespace CSharpNotion.Entities
{
    public class AliasBlock : BaseBlock
    {
        public Api.General.Pointer? AliasPointer { get; protected set; }

        public AliasBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            AliasPointer = blockValue?.Format?.AliasPointer;
        }

        public async Task SetBlockLink(string pageId, string table)
        {
            pageId = Utils.ExtractId(pageId);
            if (AliasPointer is not null && AliasPointer.Id == pageId && AliasPointer.Table == table) return;
            try
            {
                Api.General.Pointer newPointer = new(pageId, table);
                Dictionary<string, object?> args = new() { { "alias_pointer", newPointer } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                AliasPointer = newPointer;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public async Task SetBlockLink(BaseBlock block) => await SetBlockLink(block.Id, "block");
    }
}