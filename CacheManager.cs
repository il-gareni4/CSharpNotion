using CSharpNotion.Entities;

namespace CSharpNotion
{
    internal class CacheManager
    {
        private const int MaxCachedBlockCount = 250;
        private List<BaseBlock> _blocksList = new();
        private readonly Dictionary<string, BaseBlock> _blocksMap = new();

        public CacheManager()
        { }

        public BaseBlock? GetCachedBlock(string blockId)
        {
            return _blocksMap.GetValueOrDefault(blockId);
        }

        public void CacheBlock(BaseBlock block)
        {
            if (_blocksMap.ContainsKey(block.Id))
            {
                if (MaxCachedBlockCount == _blocksList.Count)
                {
                    BaseBlock removedBlock = _blocksList[0];
                    _blocksList.RemoveAt(0);
                    _blocksMap.Remove(removedBlock.Id);
                }
            }
            else _blocksList = _blocksList.Where((listBlock) => listBlock.Id != block.Id).ToList();

            _blocksList.Add(block);
            _blocksMap[block.Id] = block;
        }
    }
}