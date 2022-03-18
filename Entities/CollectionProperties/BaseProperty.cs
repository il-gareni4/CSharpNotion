using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public abstract class BaseProperty
    {
        protected Client Client { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        internal BaseProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property)
        {
            Name = property.Value.Name ?? throw new ArgumentNullException();
            Type = property.Value.Type ?? throw new ArgumentNullException();
            Client = client;
            Id = property.Key;
        }
    }
}
