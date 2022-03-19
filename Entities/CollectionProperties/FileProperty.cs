using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class FileProperty : BaseProperty
    {
        internal FileProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}
