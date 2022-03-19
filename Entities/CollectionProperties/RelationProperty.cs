using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class RelationProperty : BaseProperty
    {
        internal RelationProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}