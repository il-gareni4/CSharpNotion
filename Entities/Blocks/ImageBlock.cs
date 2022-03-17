using CSharpNotion.Api.General;

namespace CSharpNotion.Entities
{
    public class ImageBlock : FileContainingBlock<ImageBlock>
    {
        public ImageBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override ImageBlock SetFileUrl(string source)
        {
            if (source == Source) return this;
            Dictionary<string, object?> propertiesArgs = new() { { "source", new string[][] { new string[] { source } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, propertiesArgs),
                () => Source = source
            );
            Dictionary<string, object?> formatArgs = new() { { "source", source } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, formatArgs),
                () => DisplaySource = source
            );
            return this;
        }

        public override ImageBlock SetCaption(string caption)
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