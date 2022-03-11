namespace CSharpNotion.Api.Request
{
    public class SyncRecordValues
    {
        public SyncRecordElement[]? Requests { get; set; }
    }

    public class SyncRecordElement
    {
        public General.Pointer? Pointer { get; set; }
        public int Version { get; set; } = -1;
    }
}