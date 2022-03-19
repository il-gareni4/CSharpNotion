using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public abstract class BaseProperty
    {
        protected Client Client { get; set; }
        protected Collection Collection { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        internal BaseProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection)
        {
            Name = property.Value.Name ?? throw new ArgumentNullException();
            Type = property.Value.Type ?? throw new ArgumentNullException();
            Client = client;
            Collection = collection;
            Id = property.Key;
        }
    }
}
