using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class MultiselectProperty : SelectProperty
    {
        internal MultiselectProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}