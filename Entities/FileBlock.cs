using CSharpNotion.Api.Response;
using CSharpNotion.Entities.Interfaces;

namespace CSharpNotion.Entities
{
    public class FileBlock : FileContainingBlock<FileBlock>, IColorBlock<FileBlock>
    {
        public BlockColor Color { get; set; }

        public FileBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

        public override FileBlock SetFileUrl(string source)
        {
            if (source == Source) return this;
            Dictionary<string, object?> propertiesArgs = new() { { "source", new string[][] { new string[] { source } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, propertiesArgs),
                () => Source = source
            );
            return this;
        }

        public FileBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Color = color
            );
            return this;
        }

        public override FileBlock SetCaption(string caption)
        {
            if (caption == Caption) return this;
            Dictionary<string, object?> args = new() { { "caption", new string[][] { new string[] { caption } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Caption = caption
            );
            return this;
        }
    }
}