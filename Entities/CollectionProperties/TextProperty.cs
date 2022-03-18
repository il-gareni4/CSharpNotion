using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class TextProperty : BaseProperty
    {
        internal TextProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property) : base(client, property)
        {
        }
    }
}