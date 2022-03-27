using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public abstract class BaseHeaderBlock<T> : ColorTitleContentBlock<T> where T : BaseBlock
    {
        public bool Toggleable { get; set; }

        internal BaseHeaderBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Toggleable = blockValue?.Format?.Toggleable ?? false;
        }

        public abstract T SetToggleable(bool toggleable);

        public override async Task<List<BaseBlock>> GetContent()
        {
            if (!Toggleable) throw new InvalidOperationException("Header block is not toggleable");
            else return await base.GetContent();
        }

        public override U AppendBlock<U>()
        {
            if (!Toggleable) throw new InvalidOperationException("Header block is not toggleable");
            else return base.AppendBlock<U>();
        }
    }

    public class HeaderBlock : BaseHeaderBlock<HeaderBlock>
    {
        public HeaderBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override HeaderBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override HeaderBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }

        public override HeaderBlock SetToggleable(bool toggleable)
        {
            if (toggleable == Toggleable) return this;
            SetFormat("toggleable", toggleable, () => Toggleable = toggleable);
            return this;
        }
    }

    public class SubHeaderBlock : BaseHeaderBlock<SubHeaderBlock>
    {
        public SubHeaderBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override SubHeaderBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override SubHeaderBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }

        public override SubHeaderBlock SetToggleable(bool toggleable)
        {
            if (toggleable == Toggleable) return this;
            SetFormat("toggleable", toggleable, () => Toggleable = toggleable);
            return this;
        }
    }

    public class SubSubHeaderBlock : BaseHeaderBlock<SubSubHeaderBlock>
    {
        public SubSubHeaderBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override SubSubHeaderBlock SetTitle(string title)
        {
            SetProperty("title", new string[][] { new string[] { title } }, () => Title = title);
            return this;
        }

        public override SubSubHeaderBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            SetFormat("block_color", color.ToColorString(), () => Color = color);
            return this;
        }

        public override SubSubHeaderBlock SetToggleable(bool toggleable)
        {
            if (toggleable == Toggleable) return this;
            SetFormat("toggleable", toggleable, () => Toggleable = toggleable);
            return this;
        }
    }
}