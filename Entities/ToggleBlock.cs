namespace CSharpNotion.Entities
{
    public class ToggleBlock : ColorTitleContentBlock
    {
        public ToggleBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        { }
    }
}