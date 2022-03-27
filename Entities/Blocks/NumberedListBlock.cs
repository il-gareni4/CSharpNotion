using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public class NumberedListBlock : ColorTitleContentBlock<NumberedListBlock>
    {
        internal NumberedListBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override NumberedListBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override NumberedListBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }
    }
}