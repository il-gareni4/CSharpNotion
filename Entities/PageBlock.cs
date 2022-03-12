namespace CSharpNotion.Entities
{
    public class PageBlock : IconColorContentBlock
    {
        public string? Cover { get; set; }
        public float? CoverPosition { get; set; }

        public PageBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        {
            Cover = blockValue?.Format?.PageCover;
            CoverPosition = blockValue?.Format?.PageCoverPosition;
        }

        public async Task SetCover(string? pageCover)
        {
            if (pageCover == Cover) return;
            try
            {
                Dictionary<string, object?> args = new() { { "page_cover", pageCover } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Cover = pageCover;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public async Task ChangeCoverPosition(float coverPosition)
        {
            if (coverPosition < 0 || coverPosition > 1) throw new ArgumentException("The cover position must be between 0 and 1 inclusive", "coverPosition");
            if (coverPosition == CoverPosition) return;
            try
            {
                Dictionary<string, object?> args = new() { { "page_cover_position", coverPosition } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                CoverPosition = coverPosition;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}