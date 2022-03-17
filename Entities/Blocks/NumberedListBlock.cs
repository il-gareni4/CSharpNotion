using CSharpNotion.Api.General;
using CSharpNotion.Entities.Interfaces;

namespace CSharpNotion.Entities
{
    public class NumberedListBlock : ColorTitleContentBlock<NumberedListBlock>
    {
        public NumberedListBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override NumberedListBlock SetTitle(string title)
        {
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }

        public override NumberedListBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Color = color
            );
            return this;
        }
    }
}