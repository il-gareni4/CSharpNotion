using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    internal class BlocksFactory : IBlocksFactory
    {
        public BlocksFactory()
        { }

        public BaseBlock CreateBlock(string type, Client client, RecordMapBlockValue blockValue) => type switch
        {
            "page" => new PageBlock(client, blockValue),
            "text" => new TextBlock(client, blockValue),
            "header" => new HeaderBlock(client, blockValue),
            "sub_header" => new SubHeaderBlock(client, blockValue),
            "sub_sub_header" => new SubSubHeaderBlock(client, blockValue),
            "to_do" => new TodoBlock(client, blockValue),
            "bulleted_list" => new BulletedListBlock(client, blockValue),
            "numbered_list" => new NumberedListBlock(client, blockValue),
            "toggle" => new ToggleBlock(client, blockValue),
            "quote" => new QuoteBlock(client, blockValue),
            "callout" => new CalloutBlock(client, blockValue),
            "divider" => new DividerBlock(client, blockValue),
            "code" => new CodeBlock(client, blockValue),
            "alias" => new AliasBlock(client, blockValue),
            "equation" => new EquationBlock(client, blockValue),
            "transclusion_container" => new SyncContainerBlock(client, blockValue),
            "transclusion_reference" => new SyncReferenceBlock(client, blockValue),
            "breadcrumb" => new BreadcrumbBlock(client, blockValue),
            "image" => new ImageBlock(client, blockValue),
            "video" => new VideoBlock(client, blockValue),
            "audio" => new AudioBlock(client, blockValue),
            "file" => new FileBlock(client, blockValue),
            "table_of_contents" => new TableOfContentsBlock(client, blockValue),
            "collection_view_page" => new CollectionViewPageBlock(client, blockValue),
            _ => new DividerBlock(client, blockValue)
        };

        public T CreateBlock<T>(Client client, RecordMapBlockValue blockValue) where T : BaseBlock =>
            (T)CreateBlock(Constants.NotionBlockTypes[typeof(T)], client, blockValue);
    }
}