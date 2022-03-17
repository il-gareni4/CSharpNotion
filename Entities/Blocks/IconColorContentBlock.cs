using CSharpNotion.Api.General;
using CSharpNotion.Entities.Interfaces;

namespace CSharpNotion.Entities
{
    public abstract class IconColorContentBlock<T> : TitleContentBlock<T>, IColorBlock<T> where T : BaseBlock
    {
        public string? Icon { get; protected set; }
        public BlockColor Color { get; set; }

        internal IconColorContentBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Icon = blockValue?.Format?.PageIcon ?? null;
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

        public abstract T SetIcon(string? icon);

        public abstract T SetColor(BlockColor color);
    }
}