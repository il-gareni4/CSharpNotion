using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public class BulletedListBlock : ColorTitleContentBlock<BulletedListBlock>
    {
        internal BulletedListBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override BulletedListBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override BulletedListBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }
    }
}