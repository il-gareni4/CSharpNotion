namespace CSharpNotion.Entities
{
    public abstract class BaseHeaderBlock : ContentBlock
    {
        public bool Toggleable { get; set; }
        public BaseHeaderBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        {
            Toggleable = blockValue?.Format?.Toggleable ?? false;
        }

        public virtual async Task SetToggleable(bool toggleable)
        {
            if (toggleable == Toggleable) return;
            try
            {
                Dictionary<string, object?> args = new() { { "toggleable", toggleable } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Toggleable = toggleable;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public override async Task<List<BaseBlock>> GetContent()
        {
            if (!Toggleable) throw new InvalidOperationException("Header block is not toggleable");
            else return await base.GetContent();
        }

        public override async Task<T> AppendBlock<T>(string title = "")
        {
            if (!Toggleable) throw new InvalidOperationException("Header block is not toggleable");
            else return await base.AppendBlock<T>(title);
        }
    }

    public class HeaderBlock : BaseHeaderBlock
    {
        public HeaderBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        { }
    }

    public class SubHeaderBlock : BaseHeaderBlock
    {
        public SubHeaderBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        { }
    }

    public class SubSubHeaderBlock : BaseHeaderBlock
    {
        public SubSubHeaderBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        { }
    }
}
