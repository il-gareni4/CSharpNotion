using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class EquationBlock : TitleContainingBlock<EquationBlock>
    {
        public EquationBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
        }

        public override EquationBlock SetTitle(string title)
        {
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }
    }
}