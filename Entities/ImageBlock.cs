using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class ImageBlock : FileContainingBlock
    {
        public ImageBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }
    }
}