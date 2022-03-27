using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.Blocks
{
    public class ImageBlock : FileContainingBlock<ImageBlock>
    {
        internal ImageBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override ImageBlock SetFileUrl(string source)
        {
            if (source == Source) return this;
            SetProperty("soruce", new string[][] { new string[] { source } }, () => Source = source);
            SetFormat("display_source", source, () => DisplaySource = source);
            return this;
        }

        public override ImageBlock SetCaption(string caption)
        {
            if (caption == Caption) return this;
            SetProperty("caption", new string[][] { new string[] { caption } }, () => Caption = caption);
            return this;
        }
    }
}