using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public class FileBlock : FileContainingBlock<FileBlock>, IColorBlock<FileBlock>
    {
        public BlockColor Color { get; set; }

        internal FileBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

        public override FileBlock SetFileUrl(string source)
        {
            if (source == Source) return this;
            SetProperty("soruce", new string[][] { new string[] { source } }, () => Source = source);
            SetFormat("display_source", source, () => DisplaySource = source);
            return this;
        }

        public FileBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }

        public override FileBlock SetCaption(string caption)
        {
            if (caption == Caption) return this;
            SetProperty("caption", new string[][] { new string[] { caption } }, () => Caption = caption);
            return this;
        }
    }
}