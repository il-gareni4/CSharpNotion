using CSharpNotion.Api.General;
using CSharpNotion.Utilities;

namespace CSharpNotion.Entities.Blocks
{
    public class AliasBlock : BaseBlock
    {
        public Pointer? AliasPointer { get; protected set; }

        internal AliasBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            AliasPointer = blockValue?.Format?.AliasPointer;
        }

        public AliasBlock SetBlockLink(string pageId, string table)
        {
            pageId = NotionUtils.ExtractId(pageId);
            if (AliasPointer is not null && AliasPointer.Id == pageId && AliasPointer.Table == table) return this;
            Pointer newPointer = new(pageId, table);
            SetFormat("alias_pointer", newPointer, () => AliasPointer = newPointer);
            return this;
        }

        public AliasBlock SetBlockLink(BaseBlock block) => SetBlockLink(block.Id, "block");
    }
}