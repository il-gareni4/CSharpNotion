using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class CheckboxProperty : BaseProperty
    {
        internal CheckboxProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}
