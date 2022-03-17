﻿using CSharpNotion.Api.General;
using CSharpNotion.Entities.Interfaces;

namespace CSharpNotion.Entities
{
    public class TableOfContentsBlock : BaseBlock, IColorBlock<TableOfContentsBlock>
    {
        public BlockColor Color { get; set; }

        public TableOfContentsBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

        public TableOfContentsBlock SetColor(BlockColor color)
        {
            if (color == Color) return this;
            Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args),
                () => Color = color
            );
            return this;
        }
    }
}