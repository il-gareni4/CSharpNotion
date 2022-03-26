using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class PhoneNumberProperty : TextProperty
    {
        internal PhoneNumberProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
        }
    }
}