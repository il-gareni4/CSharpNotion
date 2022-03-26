using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class UrlProperty : TextProperty
    {
        internal UrlProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}