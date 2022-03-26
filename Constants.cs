using CSharpNotion.Entities.Blocks;
using CSharpNotion.Entities.CollectionProperties;

namespace CSharpNotion
{
    public class Constants
    {
        public const string BaseUrl = "https://www.notion.so";
        public const string ApiUrl = BaseUrl + "/api/v3";

        public static readonly Dictionary<Type, string> BlockTypeToTypeName = new()
        {
            { typeof(PageBlock), "page" },
            { typeof(TextBlock), "text" },
            { typeof(HeaderBlock), "header" },
            { typeof(SubHeaderBlock), "sub_header" },
            { typeof(SubSubHeaderBlock), "sub_sub_header" },
            { typeof(TodoBlock), "to_do" },
            { typeof(BulletedListBlock), "bulleted_list" },
            { typeof(NumberedListBlock), "numbered_list" },
            { typeof(ToggleBlock), "toggle" },
            { typeof(QuoteBlock), "quote" },
            { typeof(CalloutBlock), "callout" },
            { typeof(DividerBlock), "divider" },
            { typeof(CodeBlock), "code" },
            { typeof(AliasBlock), "alias" },
            { typeof(EquationBlock), "equation" },
            { typeof(SyncContainerBlock), "transclusion_container" },
            { typeof(SyncReferenceBlock), "transclusion_reference" },
            { typeof(BreadcrumbBlock), "breadcrumb" },
            { typeof(ImageBlock), "image" },
            { typeof(VideoBlock), "video" },
            { typeof(FileBlock), "file" },
            { typeof(TableOfContentsBlock), "table_of_contents" },
            { typeof(CollectionViewPageBlock), "collection_view_page" }
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