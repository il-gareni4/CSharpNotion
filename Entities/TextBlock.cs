namespace CSharpNotion.Entities
{
    public class TextBlock : ColorTitleContentBlock
    {
        public TextBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        { }
    }
}