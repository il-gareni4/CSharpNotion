namespace CSharpNotion.Api.Request
{
    internal class SyncRecordValues
    {
        public SyncRecordElement[]? Requests { get; set; }
    }

    internal class SyncRecordElement
    {
        public General.Pointer? Pointer { get; set; }
        public int Version { get; set; } = -1;
    }
}