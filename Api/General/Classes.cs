using System.Text.Json.Serialization;

namespace CSharpNotion.Api.General
{
    public class Pointer
    {
        public string Id { get; set; }
        public string Table { get; set; }

        public Pointer(string id, string table)
        { Id = id; Table = table; }
    }

    public class BlockFormat
    {
        [JsonPropertyName("copied_from_pointer")]
        public Pointer? CopiedFromPointer { get; set; }

        [JsonPropertyName("page_icon")]
        public string? PageIcon { get; set; }

        [JsonPropertyName("page_cover")]
        public string? PageCover { get; set; }

        [JsonPropertyName("page_cover_position")]
        public float? PageCoverPosition { get; set; }

        [JsonPropertyName("block_color")]
        public string? BlockColor { get; set; }

        [JsonPropertyName("display_source")]
        public string? DisplaySource { get; set; }

        [JsonPropertyName("block_width")]
        public int? BlockWidth { get; set; }

        [JsonPropertyName("block_full_width")]
        public bool? BlockFullWidth { get; set; }

        [JsonPropertyName("block_page_width")]
        public bool? BlockPageWidth { get; set; }

        [JsonPropertyName("block_aspect_ratio")]
        public float? BlockAspectRatio { get; set; }

        [JsonPropertyName("block_preserve_scale")]
        public bool? BlockPreserveScale { get; set; }

        // TODO: make enum
        [JsonPropertyName("quote_size")]
        public string? QuoteSize { get; set; }

        [JsonPropertyName("code_wrap")]
        public bool? CodeWrap { get; set; }

        [JsonPropertyName("alias_pointer")]
        public Pointer? AliasPointer { get; set; }

        public bool? Toggleable { get; set; }
    }
}