using CSharpNotion.Api.General;

namespace CSharpNotion.Api.Response
{
    internal class PageChunk
    {
        public ChunkCursor? Cursor { get; set; }
        public RecordMap? RecordMap { get; set; }
    }

    internal class ChunkCursor
    {
        public ChunkCursorElement[][] Stack { get; set; } = Array.Empty<ChunkCursorElement[]>();
    }

    internal class ChunkCursorElement
    {
        public string? Table { get; set; }
        public string? Id { get; set; }
        public int Index { get; set; }
    }


}