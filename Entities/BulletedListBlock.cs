namespace CSharpNotion.Entities
{
    public class BulletedListBlock : ColorTitleContentBlock
    {
        public BulletedListBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        { }
    }
}