using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class PersonProperty : BaseProperty
    {
        internal PersonProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}
