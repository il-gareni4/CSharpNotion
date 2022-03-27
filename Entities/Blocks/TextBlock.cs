using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public class TextBlock : ColorTitleContentBlock<TextBlock>
    {
        internal TextBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override TextBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override TextBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }
    }
}