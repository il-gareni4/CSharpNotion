using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.Blocks.Interfaces
{
    internal interface IBlocksFactory
    {
        BaseBlock CreateBlock(string type, Client client, RecordMapBlockValue blockValue);

        T CreateBlock<T>(Client client, RecordMapBlockValue blockValue) where T : BaseBlock;
    }
}