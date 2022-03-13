using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class VideoBlock : FileContainingBlock
    {
        public VideoBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }
    }
}
