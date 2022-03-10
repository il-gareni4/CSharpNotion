namespace CSharpNotion.Entities
{
    public class HeaderBlock : TitleContainingBlock
    {
        public HeaderBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        { }
    }

    public class SubHeaderBlock : TitleContainingBlock
    {
        public SubHeaderBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        { }
    }

    public class SubSubHeaderBlock : TitleContainingBlock
    {
        public SubSubHeaderBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        { }
    }
}
