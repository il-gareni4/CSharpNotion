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
    }
}