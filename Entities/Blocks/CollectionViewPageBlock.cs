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

            RecordMap recordMap = (await ReqSetup.SyncRecordValues(CollectionPointer).Send(Client.HttpClient)
                                                    .DeserializeJson<RecordMapResopnse>()).RecordMap!;
            if (recordMap.Collection is null || !recordMap.Collection.ContainsKey(CollectionId)) throw new InvalidOperationException();

            Collection = new Collection(Client, recordMap.Collection[CollectionId].Value!);
            return Collection;
        }

        public async Task<CollectionRowBlock[]> GetPages(int count)
        {
            RecordMap recordMap = (await ReqSetup.QueryCollection(CollectionId, ViewIds[0], count)
                .Send(Client.HttpClient).DeserializeJson<RecordMapResopnse>()).RecordMap!;
            if (recordMap.Block is null) throw new InvalidOperationException();
            List<CollectionRowBlock> blocks = new();
            foreach (var recordMapBlock in recordMap.Block.Values)
                blocks.Add(new CollectionRowBlock(Client, recordMapBlock.Value!, await GetCollection()));
            return blocks.ToArray();
        }

        public async Task<CollectionRowBlock> AddNewPageAsync()
        {
            RecordMapBlockValue newBlock = Utils.CreateNewBlockValue<PageBlock>(Client, SpaceId, CollectionId, "collection");
            CollectionRowBlock newBlockInstance = new(Client, newBlock, await GetCollection());
            Client.OperationsToTransaction();
            Client.AddOperation(Api.OperationBuilder.FromBlockValueToSetOperation(newBlock));
            return newBlockInstance;
        }
    }
}
