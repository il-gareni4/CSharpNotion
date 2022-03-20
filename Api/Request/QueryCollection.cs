using System.Text.Json.Serialization;

namespace CSharpNotion.Api.Request
{
    internal class QueryCollection
    {
        public IdPointer? Collection { get; set; }
        public IdPointer? CollectionView { get; set; }
        public QueryCollectionLoader? Loader { get; set; }
    }

    internal class IdPointer
    {
        public string? Id { get; set; }

        public IdPointer(string id)
        {
            Id = id;
        }
    }

    internal class QueryCollectionLoader
    {
        public LoaderReducer? Reducers { get; set; }
        public string SearchQuery { get; set; } = "";
        public LoaderSort[]? Sort { get; set; } = Array.Empty<LoaderSort>();
        public string Type { get; set; } = "reducer";
        public string UserTimeZone { get; set; } = "";
    }

    internal class LoaderReducer
    {
        [JsonPropertyName("collection_group_results")]
        public CollectionGroupResultsReducer? CollectionGroupResults { get; set; }
    }

    internal class CollectionGroupResultsReducer
    {
        public int Limit { get; set; }
        public string? Type { get; set; } = "results";
    }

    internal class LoaderSort
    {
        public string? Direction { get; set; }
        public string? Property { get; set; }
    }
}