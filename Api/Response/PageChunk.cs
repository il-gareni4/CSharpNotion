using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpNotion.Api.Response
{
    public class PageChunk
    {
        public ChunkCursor? Cursor { get; set; }
        public RecordMap? RecordMap { get; set; }
    }

    public class ChunkCursor
    {
        public ChunkCursorElement[][] Stack { get; set; } = Array.Empty<ChunkCursorElement[]>();
    }

    public class ChunkCursorElement
    {
        public string? Table { get; set; }
        public string? Id { get; set; }
        public int Index { get; set; }
    }

    public class RecordMap
    {
        public Dictionary<string, RecordMapBlock>? Block { get; set; }
    }

    public class RecordMapBlock
    {
        public string? Role { get; set; }
        public RecordMapBlockValue? Value { get; set; }
    }

    public class RecordMapBlockValue
    {
        public string? Id { get; set; }
        public int Version { get; set; }
        public string? Type { get; set; }
        public RecordMapBlockProperties? Properties { get; set; }
        public string[]? Content { get; set; }
        public RecordMapBlockPermission[]? Permissions { get; set; }

        [JsonPropertyName("created_time")]
        public long CreatedTime { get; set; }

        [JsonPropertyName("last_edited_time")]
        public long LastEditedTime { get; set; }

        [JsonPropertyName("parent_id")]
        public string? ParentId { get; set; }

        [JsonPropertyName("parent_table")]
        public string? ParentTable { get; set; }

        public bool Alive { get; set; }

        [JsonPropertyName("created_by_table")]
        public string? CreatedByTable { get; set; }

        [JsonPropertyName("created_by_id")]
        public string? CreatedById { get; set; }

        [JsonPropertyName("last_edited_by_table")]
        public string? LastEditedByTable { get; set; }

        [JsonPropertyName("last_edited_by_id")]
        public string? LastEditedById { get; set; }

        [JsonPropertyName("space_id")]
        public string? SpaceId { get; set; }

        public General.BlockFormat? Format { get; set; }
        public string[]? Discussions { get; set; }
    }

    public class RecordMapBlockPermission
    {
        public string? Role { get; set; }
        public string? Type { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }

    public class RecordMapBlockProperties
    {
        public JsonElement[][]? Title { get; set; }
        public string[][]? Checked { get; set; }
    }
}