using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties.Interfaces
{
    internal interface IPropertyFactory
    {
        BaseProperty CreateProperty(string type, Client client, KeyValuePair<string,
            CollectionValueSchemaElement> property, Collection collection);
    }
}