using CSharpNotion.Api.General;
using CSharpNotion.Entities.Interfaces;
using System.Text.Json;

namespace CSharpNotion.Entities
{
    public class PageBlock : IconColorContentBlock<PageBlock>
    {
        public string? Cover { get; set; }
        public float? CoverPosition { get; set; }
        internal Dictionary<string, JsonElement[][]> Properties { get; set; }

        internal PageBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Cover = blockValue.Format?.PageCover;
            CoverPosition = blockValue.Format?.PageCoverPosition;
            Properties = blockValue.Properties ?? new Dictionary<string, JsonElement[][]>();
        }

        public PageBlock SetCover(string? cover)
        {
            if (Cover == cover) return this;
            Dictionary<string, object?> args = new() { { "page_cover", cover } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Cover = cover
            );
            return this;
        }

        public PageBlock ChangeCoverPosition(float coverPosition)
        {
            if (coverPosition < 0 || coverPosition > 1) throw new ArgumentException("The cover position must be between 0 and 1 inclusive", "coverPosition");
            if (coverPosition == CoverPosition) return this;
            Dictionary<string, object?> args = new() { { "page_cover_position", coverPosition } };
            Client.AddOperation(
                 Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                 () => CoverPosition = coverPosition
             );
            return this;
        }

        public override PageBlock SetTitle(string title)
        {
            Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { title } } } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () => Title = title
            );
            return this;
        }

        public override PageBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Color = color
            );
            return this;
        }

        public override PageBlock SetIcon(string? icon)
        {
            if (Icon == icon) return this;
            Dictionary<string, object?> args = new() { { "page_icon", icon } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Icon = icon
            );
            return this;
        }
    }
}