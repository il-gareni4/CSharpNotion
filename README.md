# CSharpNotion - C# API client for Notion.so

This is an unofficial Notion API that uses TokenV2 to manage content on the site.

## Features

- **Fast execution of very large requests**, through transactions and operations
- **Caching**

## Installation

```console
dotnet add package CSharpNotion
```

## Usage

### Quickstart

Initialize new `Client`. `token_v2` you can obtain by inspecting browsers cookie on a logged-in session on Notion.so
![token_v2](https://i.imgur.com/6fOG6kW.jpeg)

```C#
Client client = await new Client("<token_v2>").Setup();
```

After receiving a `Client` see below

### Getting a page (`PageBlock`)

To get a block you need its ID. ID you can get from the URL string. For example:

- In the URL (page) "`https://www.notion.so/Here-goes-page-name-63192567ed7911619023252d404ded13`" ID is `63192567ed7911619023252d404ded13`. Ignore page name "`Here-goes-page-name`".

- Or in the next URL (collection) "`https://www.notion.so/fc1a3b4a7bea42b48906e993be4a7462?v=0339062f21bf4f95bbfde2428567b7fd`" ID is `fc1a3b4a7bea42b48906e993be4a7462`. Ignore URL parameters.

Once you have the block ID you can get the actual block. In the example I use page block ID

```C#
// Get a page block
PageBlock block = await client.GetBlockAsync<PageBlock>("<blockId>");

// Write block title
Console.WriteLine(block.Title);
```

`PageBlock` is one of the many types of blocks that exist in Notion. When you use `client.GetBlockAsync<Page>("<blockId>")` you already know that the block you are fetching is a `PageBlock`. If you don't know the type of the block, write

```C#
BaseBlock block = await client.GetBlockAsync("<blockId>");

// You can easily get the block type. You will get a Notion type string name
Console.WriteLine(block.Type);

// If you need to interact with this block further, then cast
// For example block.Type == "to_do"
TodoBlock todoBlock = (TodoBlock)block;
```

All supported block types can be found in the `Constants.cs` file

### Updating block properties

```C#
// page is PageBlock
await page.SetTitle("New title").Commit();
```

OR

```C#
page.SetTitle("New Title");
await client.Commit();
```

When you call `page.SetTitle(string)` you are creating an operation to change the title. That is, if you call `page.Title` after `page.SetTitle(string)`, then the title will not change, since the request to the server has not yet been sent. To commit changes and request to the server, you need to call `Client.Commit()` or `BaseBlock.Commit()`. This makes it possible to make very large requests extremely quickly.

Because of this, you can write change chains

```C#
// todo is TodoBlock
await todo.SetTitle("Todo title").SetChecked(true).SetColor(BlockColor.Orange).Commit();
```

What properties of which block has you can see through IntelliSense or in the source code.

### Blocks content

Some of the blocks may contain child blocks (`PageBlock`, `BulletedListBlock`, etc.). Base class is `ContentBlock`.

If you need only child block IDs, write `contentBlock.ContentIds`.

Otherwise, if you want the actual blocks, call

```C#
// GetContent() is an asynchronous method because it makes requests to the server
List<BaseBlock> childBlocks = await page.GetContent();
```

### Inserting and removing blocks

#### **Insert**

There are several methods to add new blocks: `AppendBlock`, `InsertBlock`. And `BaseBlock` contains method `InsertBlockAround`.

```C#
// Append a new block to the bottom of the page
TextBlock textBlock = page.AppendBlock<TextBlock>();

// Notion servers do not yet contain this block, but you can already change its properties
textBlock.SetTitle("Some title");

// Don't forget to commit appending new block!
await client.Commit();
```

```C#
// Insert block at the top of the page
CalloutBlock calloutBlock = page.InsertBlock<CalloutBlock>(0).SetIcon("😀");
await client.Commit();
```

```C#
// divider is DividerBlock
// Add new block after current block
await divider.InsertBlockAround<CodeBlock>(ListCommand.listAfter).Commit();
```

#### **Remove**

```C#
// Remove third block
page.RemoveBlock(2);

// Remove block by ID
page.RemoveBlock("63192567-ed29-4161-9023-251d404ded80");

// Remove all blocks
page.RemoveBlocks(page.ContentIds);

await client.Commit();
```
