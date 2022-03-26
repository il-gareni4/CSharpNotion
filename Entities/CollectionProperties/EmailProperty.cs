using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class EmailProperty : TextProperty
    {
        internal EmailProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}