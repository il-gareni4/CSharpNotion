using CSharpNotion.Api.Response;
using CSharpNotion.Entities.Interfaces;

namespace CSharpNotion.Entities
{
    public abstract class BaseHeaderBlock<T> : ColorTitleContentBlock<T> where T : BaseBlock
    {
        public bool Toggleable { get; set; }

        public BaseHeaderBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
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
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }

        public override HeaderBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Color = color
            );
            return this;
        }

        public override HeaderBlock SetToggleable(bool toggleable)
        {
            if (toggleable == Toggleable) return this;
            Dictionary<string, object?> args = new() { { "toggleable", toggleable } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Toggleable = toggleable
            );
            return this;
        }
    }

    public class SubHeaderBlock : BaseHeaderBlock<SubHeaderBlock>
    {
        public SubHeaderBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override SubHeaderBlock SetTitle(string title)
        {
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }

        public override SubHeaderBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Color = color
            );
            return this;
        }

        public override SubHeaderBlock SetToggleable(bool toggleable)
        {
            if (toggleable == Toggleable) return this;
            Dictionary<string, object?> args = new() { { "toggleable", toggleable } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Toggleable = toggleable
            );
            return this;
        }
    }

    public class SubSubHeaderBlock : BaseHeaderBlock<SubSubHeaderBlock>
    {
        public SubSubHeaderBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        { }

        public override SubSubHeaderBlock SetTitle(string title)
        {
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }

        public override SubSubHeaderBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Color = color
            );
            return this;
        }

        public override SubSubHeaderBlock SetToggleable(bool toggleable)
        {
            if (toggleable == Toggleable) return this;
            Dictionary<string, object?> args = new() { { "toggleable", toggleable } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Toggleable = toggleable
            );
            return this;
        }
    }
}