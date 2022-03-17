using CSharpNotion.Api.General;
using CSharpNotion.Entities.Interfaces;

namespace CSharpNotion.Entities
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
            Dictionary<string, object?> args = new() { { "quote_size", size == FormatQuoteSize.default_size ? null : "large" } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => QuoteSize = size
            );
            return this;
        }

        public override QuoteBlock SetTitle(string title)
        {
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }

        public override QuoteBlock SetColor(BlockColor color)
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