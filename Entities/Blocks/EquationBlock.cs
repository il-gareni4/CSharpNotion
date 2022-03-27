using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.Blocks
{
    public class EquationBlock : TitleContainingBlock<EquationBlock>
    {
        internal EquationBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
        }

        public override EquationBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }
    }
}