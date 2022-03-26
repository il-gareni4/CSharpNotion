using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public class TodoBlock : ColorTitleContentBlock<TodoBlock>
    {
        public bool Checked { get; private set; }

        internal TodoBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            string? checkedValue = blockValue?.Properties?.GetValueOrDefault("checked")?.ElementAt(0)[0].GetString();
            Checked = checkedValue == "Yes";
        }

        public TodoBlock ToggleChecked() => SetChecked(!Checked);

        public TodoBlock SetChecked(bool checkedState)
        {
            if (checkedState == Checked) return this;
            Dictionary<string, object?> args = new() { { "checked", new string[][] { new string[] { checkedState ? "Yes" : "No" } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Checked = checkedState
            );
            return this;
        }

        public override TodoBlock SetTitle(string title)
        {
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }

        public override TodoBlock SetColor(BlockColor color)
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