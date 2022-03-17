using CSharpNotion.Api.General;

namespace CSharpNotion.Entities
{
    public class CollectionViewPageBlock : BaseBlock
    {
        public string[] ViewIds { get; protected set; }
        public string CollectionId { get; protected set; }
        public Pointer CollectionPointer { get; protected set; }

        public CollectionViewPageBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            ViewIds = blockValue?.ViewIds ?? Array.Empty<string>();
            CollectionId = blockValue?.CollectionId ?? throw new ArgumentNullException();
            CollectionPointer = blockValue?.Format?.CollectionPointer ?? throw new ArgumentNullException();
        }
    }
}
