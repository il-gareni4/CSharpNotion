using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class NumberProperty : BaseProperty
    {
        public string? NumberFormat { get; set; }

        internal NumberProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
            NumberFormat = property.Value.NumberFormat;
        }
    }
}