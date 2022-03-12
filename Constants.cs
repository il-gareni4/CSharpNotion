namespace CSharpNotion
{
    public class Constants
    {
        public const string BaseUrl = "https://www.notion.so";
        public const string ApiUrl = BaseUrl + "/api/v3";

        public static readonly Dictionary<Type, string> BlockToType = new()
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
            { typeof(Entities.FileBlock), "file" }
        };

        public static readonly Dictionary<string, Type> TypeToBlock =
            new(BlockToType.Select((pair) => new KeyValuePair<string, Type>(pair.Value, pair.Key)));
    }
}