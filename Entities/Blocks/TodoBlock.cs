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
            SetProperty("checked", new string[][] { new string[] { checkedState ? "Yes" : "No" } }, () => Checked = checkedState);
            return this;
        }

        public override TodoBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override TodoBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }
    }
}