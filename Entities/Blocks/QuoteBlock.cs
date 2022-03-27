using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public enum FormatQuoteSize
    {
        default_size,
        large
    }

    public class QuoteBlock : ColorTitleContentBlock<QuoteBlock>
    {
        public FormatQuoteSize QuoteSize { get; private set; }

        internal QuoteBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            QuoteSize = blockValue?.Format?.QuoteSize switch
            {
                "large" => FormatQuoteSize.large,
                _ => FormatQuoteSize.default_size
            };
        }

        public QuoteBlock SetQuoteSize(FormatQuoteSize size)
        {
            if (size == QuoteSize) return this;
            SetFormat("quote_size", size == FormatQuoteSize.default_size ? null : "large", () => QuoteSize = size);
            return this;
        }

        public override QuoteBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override QuoteBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }
    }
}