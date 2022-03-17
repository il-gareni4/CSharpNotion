using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpNotion.Api.Response
{
    internal class SyncRecordValuesResponse
    {
        public SyncRecordMap? RecordMap { get; set; }
    }

    internal class SyncRecordMap
    {
        [JsonPropertyName("space_view")]
        public Dictionary<string, object>? SpaceView { get; set; }

        public Dictionary<string, RecordMapBlock>? Block { get; set; }

        public Dictionary<string, RecordMapCollection>? Collection { get; set; }
    }

    internal class RecordMapCollection
    {
        public string? Role { get; set; }
        public RecordMapCollectionValue? Value { get; set; }
    }

    internal class RecordMapCollectionValue
    {
        public string? Id { get; set; }
        public int Version { get; set; }
        public JsonElement[][]? Name { get; set; }
        public Dictionary<string, CollectionValueSchemaElement>? Schema { get; set; }
        public string? ParentId { get; set; }
        public string? ParentTable { get; set; }
        public bool Alive { get; set; }
        public bool Migrated { get; set; }
        public string? SpaceId { get; set; }
    }

    internal class CollectionValueSchemaElement
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public CollectionSelectOption[]? Options { get; set; }

        [JsonPropertyName("number_format")]
        public string? NumberFormat { get; set; }

        [JsonPropertyName("date_format")]
        public string? DateFormat { get; set; }

        [JsonPropertyName("time_format")]
        public string? TimeFormat { get; set; }

        public string? Property { get; set; }
        public string? CollectionId { get; set; }

        [JsonPropertyName("collection_pointer")]
        public General.Pointer? CollectionPointer { get; set; }

        public string? Aggregation { get; set; }

        [JsonPropertyName("target_property")]
        public string? TargetProperty { get; set; }

        [JsonPropertyName("relation_property")]
        public string? RelationProperty { get; set; }

        [JsonPropertyName("target_property_type")]
        public string? TargetPropertyType { get; set; }
    }

    internal class CollectionSelectOption
    {
        public string? Id { get; set; }
        public string? Color { get; set; }
        public string? Value { get; set; }
    }
}