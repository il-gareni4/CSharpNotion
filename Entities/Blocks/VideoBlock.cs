using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.Blocks
{
    public class VideoBlock : FileContainingBlock<VideoBlock>
    {
        internal VideoBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override VideoBlock SetFileUrl(string source)
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

        public override VideoBlock SetCaption(string caption)
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