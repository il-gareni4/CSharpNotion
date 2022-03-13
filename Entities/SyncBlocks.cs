using CSharpNotion.Api.Response;

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

        public SyncReferenceBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            SyncContainerPointer = blockValue?.Format?.TransclusionReferencePointer;
            BlockValue = blockValue!;
        }

        private async Task FetchSyncContainerBlock()
        {
            if (SyncContainerPointer is null) throw new NullReferenceException();
            if (SyncContainerBlock is not null) return;

            SyncContainerBlock = await Client.GetBlockAsync<SyncContainerBlock>(SyncContainerPointer.Id);
            ContentIds = SyncContainerBlock.ContentIds;
        }

        public override async Task<List<BaseBlock>> GetContent()
        {
            await FetchSyncContainerBlock();
            return await SyncContainerBlock!.GetContent();
        }

        public override async Task<T> AppendBlock<T>(string title = "")
        {
            await FetchSyncContainerBlock();
            return await SyncContainerBlock!.AppendBlock<T>(title);
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