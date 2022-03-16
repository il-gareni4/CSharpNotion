using CSharpNotion.Api.Response;

namespace CSharpNotion.Api.Request
{
    internal class LoadCachedPageChunk
    {
        public LoadCachedPageChunkPage? Page { get; set; }
        public int Limit { get; set; } = 30;
        public ChunkCursor Cursor { get; set; } = new();
        public int ChunkNumber { get; set; }
        public bool VerticalColumns { get; set; }
    }

    internal class LoadCachedPageChunkPage
    {
        public string? Id { get; set; }
    }

    internal class LoadCachedPageChunkCursor
    {
        public string[] Stack { get; set; } = Array.Empty<string>();
    }
}