using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public class CalloutBlock : IconColorContentBlock<CalloutBlock>
    {
        internal CalloutBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override CalloutBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override CalloutBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }

        public override CalloutBlock SetIcon(string? icon)
        {
            if (Icon == icon) return this;
            SetFormat("page_icon", icon, () => Icon = icon);
            return this;
        }
    }
}