using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class DateProperty : BaseProperty
    {
        internal DateProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}