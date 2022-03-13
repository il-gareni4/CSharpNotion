namespace CSharpNotion.Entities
{
    public class EquationBlock : TitleContainingBlock
    {
        public EquationBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
        }
    }
}