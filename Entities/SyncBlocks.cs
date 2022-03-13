namespace CSharpNotion.Entities
{
    public class SyncContainerBlock : ContentBlock
    {
        public SyncContainerBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        { }
    }

    public class SyncReferenceBlock : ContentBlock
    {
        public Api.General.Pointer? SyncContainerPointer { get; protected set; }
        protected SyncContainerBlock? SyncContainerBlock { get; set; }

        public SyncReferenceBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            SyncContainerPointer = blockValue?.Format?.TransclusionReferencePointer;
        }

        private async Task FetchSyncContainerBlock()
        {
            if (SyncContainerPointer is null) throw new NullReferenceException();
            if (SyncContainerBlock is not null) return;

            SyncContainerBlock = await Client.GetBlockAsync<SyncContainerBlock>(Client.HttpClient, SyncContainerPointer.Id);
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

        public async Task SetSyncBlock(string pageId)
        {
            pageId = Utils.ExtractId(pageId);
            if (SyncContainerPointer is not null && SyncContainerPointer.Id == pageId && SyncContainerPointer.Table == "block") return;
            try
            {
                Api.General.Pointer newPointer = new(pageId, "block");
                Dictionary<string, object?> args = new() { { "transclusion_reference_pointer", newPointer } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                SyncContainerPointer = newPointer;
                SyncContainerBlock = null;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public async Task SetSyncBlock(SyncContainerBlock syncContainerBlock) => await SetSyncBlock(syncContainerBlock.Id);

        public async Task SetSyncBlock(SyncReferenceBlock syncReferenceBlock)
        {
            if (syncReferenceBlock.SyncContainerPointer is null) throw new InvalidDataException();
            await SetSyncBlock(syncReferenceBlock.SyncContainerPointer.Id);
        }
    }
}