using CSharpNotion.Entities.CollectionProperties;

namespace CSharpNotion
{
    public class Constants
    {
        public const string BaseUrl = "https://www.notion.so";
        public const string ApiUrl = BaseUrl + "/api/v3";

        public static readonly Dictionary<Type, string> BlockTypeToTypeName = new()
        {
            { typeof(Entities.PageBlock), "page" },
            { typeof(Entities.TextBlock), "text" },
            { typeof(Entities.HeaderBlock), "header" },
            { typeof(Entities.SubHeaderBlock), "sub_header" },
            { typeof(Entities.SubSubHeaderBlock), "sub_sub_header" },
            { typeof(Entities.TodoBlock), "to_do" },
            { typeof(Entities.BulletedListBlock), "bulleted_list" },
            { typeof(Entities.NumberedListBlock), "numbered_list" },
            { typeof(Entities.ToggleBlock), "toggle" },
            { typeof(Entities.QuoteBlock), "quote" },
            { typeof(Entities.CalloutBlock), "callout" },
            { typeof(Entities.DividerBlock), "divider" },
            { typeof(Entities.CodeBlock), "code" },
            { typeof(Entities.AliasBlock), "alias" },
            { typeof(Entities.EquationBlock), "equation" },
            { typeof(Entities.SyncContainerBlock), "transclusion_container" },
            { typeof(Entities.SyncReferenceBlock), "transclusion_reference" },
            { typeof(Entities.BreadcrumbBlock), "breadcrumb" },
            { typeof(Entities.ImageBlock), "image" },
            { typeof(Entities.VideoBlock), "video" },
            { typeof(Entities.FileBlock), "file" },
            { typeof(Entities.TableOfContentsBlock), "table_of_contents" },
            { typeof(Entities.CollectionViewPageBlock), "collection_view_page" }
        };

        public static readonly Dictionary<string, Type> TypeNameToBlockType =
            new(BlockTypeToTypeName.Select((pair) => new KeyValuePair<string, Type>(pair.Value, pair.Key)));

        public static readonly Dictionary<string, Type> TypeNameToPropertyType = new()
        {
            { "text", typeof(TextProperty) },
            { "number", typeof(NumberProperty) },
            { "url", typeof(UrlProperty) },
            { "email", typeof(EmailProperty) },
            { "phone_number", typeof(PhoneNumberProperty) },
            { "checkbox", typeof(CheckboxProperty) },
            { "select", typeof(SelectProperty) },
            { "multi_select", typeof(MultiselectProperty) },
            { "person", typeof(PersonProperty) },
            { "relation", typeof(RelationProperty) },
            { "file", typeof(FileProperty) },
            { "date", typeof(DateProperty) }
        };
    }
}