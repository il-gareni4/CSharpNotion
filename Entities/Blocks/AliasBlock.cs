using CSharpNotion.Api.General;

namespace CSharpNotion.Entities
{
    public class AliasBlock : BaseBlock
    {
        public Api.General.Pointer? AliasPointer { get; protected set; }

        public AliasBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            AliasPointer = blockValue?.Format?.AliasPointer;
        }

        public AliasBlock SetBlockLink(string pageId, string table)
        {
            pageId = Utils.ExtractId(pageId);
            if (AliasPointer is not null && AliasPointer.Id == pageId && AliasPointer.Table == table) return this;
            Api.General.Pointer newPointer = new(pageId, table);
            Dictionary<string, object?> args = new() { { "alias_pointer", newPointer } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => AliasPointer = newPointer
            );
            return this;
        }

        public AliasBlock SetBlockLink(BaseBlock block) => SetBlockLink(block.Id, "block");
    }
}