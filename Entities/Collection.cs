using CSharpNotion.Api.General;
using CSharpNotion.Entities.CollectionProperties;

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
        public List<BaseProperty> Properties { get; protected set; }
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

            Alive = collectionValue.Alive;
            Version = collectionValue.Version;
            Migrated = collectionValue.Migrated;

            PropertyFactory propertyFactory = new();
            Properties = (from keyValue in collectionValue.Schema
                select propertyFactory.CreateProperty(keyValue.Value.Name!, client, keyValue, this)).ToList();
        }

        internal BaseProperty? GetPropertyByName(string name)
        {
            return Properties.FirstOrDefault(prop => prop.Name == name);
        }
    }
}