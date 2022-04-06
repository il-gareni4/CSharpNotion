using CSharpNotion.Entities.Blocks;
using System.Text.Json;

namespace CSharpNotion
{
    public class Constants
    {
        public const string BaseUrl = "https://www.notion.so";
        public const string ApiUrl = BaseUrl + "/api/v3";

        public static readonly JsonSerializerOptions SerializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static readonly Dictionary<Type, string> NotionBlockTypes = new()
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
            { typeof(AudioBlock), "audio" },
            { typeof(FileBlock), "file" },
            { typeof(TableOfContentsBlock), "table_of_contents" },
            { typeof(CollectionViewPageBlock), "collection_view_page" }
        };
    }
}