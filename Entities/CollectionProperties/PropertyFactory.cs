using CSharpNotion.Api.General;
using CSharpNotion.Entities.CollectionProperties.Interfaces;

namespace CSharpNotion.Entities.CollectionProperties
{
    internal class PropertyFactory : IPropertyFactory
    {
        public PropertyFactory()
        { }

        public BaseProperty CreateProperty(string type, Client client, KeyValuePair<string,
            CollectionValueSchemaElement> property, Collection collection) => type switch
            {
                "text" => new TextProperty(client, property, collection),
                "number" => new NumberProperty(client, property, collection),
                "url" => new UrlProperty(client, property, collection),
                "email" => new EmailProperty(client, property, collection),
                "phone_number" => new PhoneNumberProperty(client, property, collection),
                "checkbox" => new CheckboxProperty(client, property, collection),
                "select" => new SelectProperty(client, property, collection),
                "multi_select" => new MultiselectProperty(client, property, collection),
                "person" => new PersonProperty(client, property, collection),
                "relation" => new RelationProperty(client, property, collection),
                "file" => new FileProperty(client, property, collection),
                "date" => new DateProperty(client, property, collection),
                _ => new TextProperty(client, property, collection)
            };
    }
}