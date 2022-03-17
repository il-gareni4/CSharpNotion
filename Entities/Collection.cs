using CSharpNotion.Api.General;

namespace CSharpNotion.Entities
{
    public class Collection
    {
        protected Client Client { get; set; }
        public string Id { get; protected set; }
        public int Version { get; protected set; }
        public string Name { get; protected set; }
        public string ParentId { get; protected set; }
        public string ParentTable { get; protected set; }
        public Dictionary<string, CollectionValueSchemaElement> Schema { get; protected set; }
        public bool Alive { get; protected set; }
        public bool Migrated { get; protected set; }
        public string SpaceId { get; protected set; }

        public Collection(Client client, RecordMapCollectionValue collectionValue)
        {
            Client = client;
            Id = collectionValue.Id ?? throw new ArgumentNullException();
            ParentId = collectionValue.ParentId ?? throw new ArgumentNullException();
            ParentTable = collectionValue.ParentTable ?? throw new ArgumentNullException();
            SpaceId = collectionValue.SpaceId ?? throw new ArgumentNullException();
            Name = collectionValue.Name?.ElementAt(0)[0].GetString() ?? string.Empty;
            Schema = collectionValue.Schema ?? new Dictionary<string, CollectionValueSchemaElement>();

            Alive = collectionValue.Alive;
            Version = collectionValue.Version;
            Migrated = collectionValue.Migrated;
        }
    }
}