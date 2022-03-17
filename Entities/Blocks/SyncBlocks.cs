using CSharpNotion.Api.General;

namespace CSharpNotion.Entities
{
    public class SyncContainerBlock : ContentBlock
    {
        public SyncContainerBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }
    }

    public class SyncReferenceBlock : ContentBlock
    {
        public Api.General.Pointer? SyncContainerPointer { get; protected set; }
        protected SyncContainerBlock? SyncContainerBlock { get; set; }
        public RecordMapBlockValue BlockValue { get; }

        internal SyncReferenceBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            SyncContainerPointer = blockValue?.Format?.TransclusionReferencePointer;
            BlockValue = blockValue!;
        }

        public async Task FetchSyncContainerBlock()
        {
            if (SyncContainerPointer is null) throw new NullReferenceException();
            if (SyncContainerBlock is not null) return;

            SyncContainerBlock = await Client.GetBlockAsync<SyncContainerBlock>(SyncContainerPointer.Id);
            ContentIds = SyncContainerBlock.ContentIds;
        }

        private void CheckSyncContainerFetched()
        {
            if (SyncContainerBlock is null)
            {
                throw new InvalidOperationException("You need to fetch a SyncContainerBlock" +
                " before using ContentBlock (base class) methods. You can do it by calling method FetchSyncContainerBlock()");
            }
        }

        public override async Task<List<BaseBlock>> GetContent()
        {
            CheckSyncContainerFetched();
            return await SyncContainerBlock!.GetContent();
        }

        public override T AppendBlock<T>()
        {
            CheckSyncContainerFetched();
            return SyncContainerBlock!.AppendBlock<T>();
        }

        public SyncReferenceBlock SetSyncBlock(string pageId)
        {
            pageId = Utils.ExtractId(pageId);
            if (SyncContainerPointer is not null && SyncContainerPointer.Id == pageId && SyncContainerPointer.Table == "block") return this;
            Api.General.Pointer newPointer = new(pageId, "block");
            Dictionary<string, object?> args = new() { { "transclusion_reference_pointer", newPointer } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () =>
                {
                    SyncContainerPointer = newPointer;
                    SyncContainerBlock = null;
                }
            );
            return this;
        }

        public SyncReferenceBlock SetSyncBlock(SyncContainerBlock syncContainerBlock) => SetSyncBlock(syncContainerBlock.Id);

        public SyncReferenceBlock SetSyncBlock(SyncReferenceBlock syncReferenceBlock)
        {
            if (syncReferenceBlock.SyncContainerPointer is null) throw new InvalidDataException();
            return SetSyncBlock(syncReferenceBlock.SyncContainerPointer.Id);
        }
    }
}