using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class TextProperty : BaseProperty
    {
        internal TextProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}