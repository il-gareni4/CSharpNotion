using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public class PageBlock : IconColorContentBlock<PageBlock>
    {
        public string? Cover { get; set; }
        public float? CoverPosition { get; set; }

        internal PageBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Cover = blockValue.Format?.PageCover;
            CoverPosition = blockValue.Format?.PageCoverPosition;
        }

        public PageBlock SetCover(string? cover)
        {
            if (Cover == cover) return this;
            SetFormat("page_cover", cover, () => Cover = cover);
            return this;
        }

        public PageBlock ChangeCoverPosition(float coverPosition)
        {
            if (coverPosition < 0 || coverPosition > 1) throw new ArgumentException("The cover position must be between 0 and 1 inclusive", "coverPosition");
            if (coverPosition == CoverPosition) return this;
            SetFormat("page_cover_position", coverPosition, () => CoverPosition = coverPosition);
            return this;
        }

        public override PageBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override PageBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }

        public override PageBlock SetIcon(string? icon)
        {
            if (Icon == icon) return this;
            SetFormat("page_icon", icon, () => Icon = icon);
            return this;
        }
    }
}