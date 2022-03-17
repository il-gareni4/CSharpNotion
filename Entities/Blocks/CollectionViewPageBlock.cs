using CSharpNotion.Api.General;
using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class CollectionViewPageBlock : BaseBlock
    {
        public string[] ViewIds { get; protected set; }
        public string CollectionId { get; protected set; }
        public Pointer CollectionPointer { get; protected set; }
        protected Collection? Collection { get; set; }

        internal CollectionViewPageBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            ViewIds = blockValue?.ViewIds ?? Array.Empty<string>();
            CollectionId = blockValue?.CollectionId ?? throw new ArgumentNullException();
            CollectionPointer = blockValue?.Format?.CollectionPointer ?? throw new ArgumentNullException();
        }

        public async Task<Collection> GetCollection()
        {
            if (Collection is not null) return Collection;

            RecordMap recordMap = (await QuickRequestSetup.SyncRecordValues(CollectionPointer).Send(Client.HttpClient)
                                                    .DeserializeJson<RecordMapResopnse>()).RecordMap!;
            if (recordMap.Collection is null || !recordMap.Collection.ContainsKey(CollectionId)) throw new InvalidOperationException();

            Collection = new Collection(Client, recordMap.Collection[CollectionId].Value!);
            return Collection;
        }

        public async Task<PageBlock[]> GetPages(int count)
        {
            RecordMap recordMap = (await QuickRequestSetup.QueryCollection(CollectionId, ViewIds[0], count)
                .Send(Client.HttpClient).DeserializeJson<RecordMapResopnse>()).RecordMap!;
            if (recordMap.Block is null) throw new ArgumentNullException();
            List<PageBlock> blocks = new List<PageBlock>();
            foreach (RecordMapBlock recordMapBlock in recordMap.Block.Values)
                blocks.Add(new PageBlock(Client, recordMapBlock.Value!));
            return blocks.ToArray();
        }
    }
}
