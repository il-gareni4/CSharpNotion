using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks.Interfaces;

namespace CSharpNotion.Entities.Blocks
{
    public abstract class BaseBlock
    {
        protected Client Client { get; set; }
        public string Id { get; init; }
        public int Version { get; protected set; }
        public string Type { get; protected set; }
        public RecordMapBlockPermission[]? Permissions { get; protected set; }
        public DateTime? CreatedTime { get; protected set; }
        public DateTime? LastEditedTime { get; protected set; }
        public bool Alive { get; protected set; }
        public string? CreatedByTable { get; protected set; }
        public string? CreatedById { get; protected set; }
        public string? LastEditedByTable { get; protected set; }
        public string? LastEditedById { get; protected set; }
        public string ParentId { get; protected set; }
        public string ParentTable { get; protected set; }
        public string SpaceId { get; protected set; }

        protected BaseBlock(Client client, RecordMapBlockValue blockValue)
        {
            Client = client;
            Type = blockValue.Type ?? throw new ArgumentNullException();
            Id = blockValue.Id ?? throw new ArgumentNullException();
            ParentId = blockValue.ParentId ?? throw new ArgumentNullException();
            ParentTable = blockValue.ParentTable ?? throw new ArgumentNullException();
            SpaceId = blockValue.SpaceId ?? throw new ArgumentNullException();

            Alive = blockValue.Alive;
            Version = blockValue.Version;
            Permissions = blockValue?.Permissions;
            CreatedTime = blockValue?.CreatedTime is not null ? DateTimeOffset.FromUnixTimeMilliseconds(blockValue.CreatedTime).DateTime : null;
            LastEditedTime = blockValue?.LastEditedTime is not null ? DateTimeOffset.FromUnixTimeMilliseconds(blockValue.LastEditedTime).DateTime : null;
            CreatedByTable = blockValue?.CreatedByTable;
            CreatedById = blockValue?.CreatedById;
            LastEditedByTable = blockValue?.LastEditedByTable;
            LastEditedById = blockValue?.LastEditedById;
        }

        /// <summary>
        /// The same as <see cref="Client.Commit"/>
        /// </summary>
        public virtual async Task Commit() => await Client.Commit();

        /// <summary>
        /// Creates a new operation that
        /// inserts a new <typeparamref name="T"/> before or after the current block
        /// </summary>
        /// <remarks>
        /// This methods works only if <see cref="ParentTable"/><c> == 'block'</c>
        /// </remarks>
        /// <typeparam name="T">New block type</typeparam>
        /// <param name="whereInsert">Specifies where to insert the new block</param>
        /// <returns>New <typeparamref name="T"/></returns>
        /// <exception cref="InvalidOperationException"><see cref="ParentTable"/> is not 'block'</exception>
        public virtual T InsertBlockAround<T>(Api.ListCommand whereInsert) where T : BaseBlock
        {
            if (ParentTable != "block") throw new InvalidOperationException("ParentTable is not 'block'");
            Client.OperationsToTransaction();
            RecordMapBlockValue newBlock = Utils.CreateNewBlockValue<T>(Client, SpaceId, ParentId);
            T newBlockInstance = Utils.ActivatorCreateNewBlock<T>(Client, newBlock);
            Client.AddOperation(Api.OperationBuilder.FromBlockValueToSetOperation(newBlock));
            Client.AddOperation(
                Api.OperationBuilder.ListInsertingOperation(whereInsert, ParentId, newBlock.Id!, Id),
                () => Client.CacheBlock(newBlockInstance)
            );
            return newBlockInstance;
        }

        public override string ToString()
        {
            return $"<{GetType().Name}, {Id}>";
        }
    }

    public abstract class TitleContainingBlock<T> : BaseBlock, ITitleBlock<T> where T : BaseBlock
    {
        public string Title { get; protected set; }

        protected TitleContainingBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Title = Utils.RecieveTitle(blockValue);
        }

        public abstract T SetTitle(string title);

        public override string ToString()
        {
            return $"<{GetType().Name}, {Id}, Title: \"{Title}\">";
        }
    }

    /// <summary>
    /// Represents a block that is capable of owning child blocks
    /// </summary>
    public abstract class ContentBlock : BaseBlock
    {
        protected List<BaseBlock> Content { get; set; } = new List<BaseBlock>();

        /// <summary>
        /// IDs of child blocks
        /// </summary>
        public List<string> ContentIds { get; protected set; }

        protected ContentBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            ContentIds = blockValue.Content?.ToList() ?? new List<string>();
        }

        /// <summary>
        /// Asynchronously fetches all blocks by <see cref="ContentIds"/>
        /// </summary>
        /// <remarks>
        /// You can always explicitly cast <see cref="BaseBlock"/> to any other child class. Check <see cref="BaseBlock.Type"/> before casting to avoid errors.
        /// </remarks>
        /// <returns><see cref="List{T}"/> of <see cref="BaseBlock"/></returns>
        /// <exception cref="InvalidDataException">Server response is invalid</exception>
        public virtual async Task<List<BaseBlock>> GetContent()
        {
            if (Content.Count != ContentIds.Count) Content = await Client.GetBlocksAsync(ContentIds);
            return Content;
        }

        /// <summary>
        /// Creates a new operation that
        /// appends a new <typeparamref name="T"/> at the end of the <see cref="ContentBlock"/>
        /// </summary>
        /// <typeparam name="T">Type of block</typeparam>
        /// <returns>New <typeparamref name="T"/></returns>
        public virtual T AppendBlock<T>() where T : BaseBlock
        {
            RecordMapBlockValue newBlock = Utils.CreateNewBlockValue<T>(Client, SpaceId, Id);
            T newBlockInstance = Utils.ActivatorCreateNewBlock<T>(Client, newBlock);
            Client.OperationsToTransaction();
            Client.AddOperation(Api.OperationBuilder.FromBlockValueToSetOperation(newBlock));
            Client.AddOperation(
                Api.OperationBuilder.ListInsertingOperation(Api.ListCommand.listAfter, Id, newBlock.Id!, ContentIds.LastOrDefault()),
                () =>
                {
                    ContentIds.Add(newBlock.Id!);
                    Content.Add(newBlockInstance);
                    Client.CacheBlock(newBlockInstance);
                }
            );
            return newBlockInstance;
        }

        /// <summary>
        /// Creates a new operation that
        /// inserts a new <typeparamref name="T"/> before or after the child block with <paramref name="blockId"/>.
        /// </summary>
        /// <typeparam name="T">Type of block.</typeparam>
        /// <param name="whereInsert">Specifies where to insert the new block.</param>
        /// <param name="blockId">ID of child block.</param>
        /// <returns>New <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Block with <paramref name="blockId"/> isn't a child of current block.</exception>
        public virtual T InsertBlock<T>(Api.ListCommand whereInsert, string blockId) where T : BaseBlock
        {
            blockId = Utils.ExtractId(blockId);
            int relativeBlockIndex = ContentIds.IndexOf(blockId);
            if (relativeBlockIndex == -1) throw new ArgumentException("Block with that ID isn't a child of current block", nameof(blockId));

            if (whereInsert == Api.ListCommand.listAfter) relativeBlockIndex++;
            return InsertBlock<T>(relativeBlockIndex);
        }

        /// <summary>
        /// Creates a new operation that
        /// inserts a new <typeparamref name="T"/> at <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="T">Type of block.</typeparam>
        /// <param name="index">The zero-based index at which block should be inserted.</param>
        /// <returns>New <typeparamref name="T"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">index is less than 0. -or- index is equal or greater than <see cref="ContentIds"/> Count</exception>
        public virtual T InsertBlock<T>(int index) where T : BaseBlock
        {
            if (index == ContentIds.Count) throw new IndexOutOfRangeException("If you want to append block to the end of content, use AppendBlock<T>()");
            else if (index > ContentIds.Count || index < 0) throw new IndexOutOfRangeException();

            string? blockId = ContentIds.Count == 0 ? null : ContentIds[index];
            RecordMapBlockValue newBlock = Utils.CreateNewBlockValue<T>(Client, SpaceId, Id);
            T newBlockInstance = Utils.ActivatorCreateNewBlock<T>(Client, newBlock);
            Client.OperationsToTransaction();
            Client.AddOperation(Api.OperationBuilder.FromBlockValueToSetOperation(newBlock));
            Client.AddOperation(
                Api.OperationBuilder.ListInsertingOperation(Api.ListCommand.listBefore, Id, newBlock.Id!, blockId),
                () =>
                {
                    ContentIds.Insert(index, newBlock.Id!);
                    if (Content.Count > index) Content.Insert(index, newBlockInstance);
                    Client.CacheBlock(newBlockInstance);
                }
            );
            return newBlockInstance;
        }

        /// <summary>
        /// Creates a new operation that
        /// deletes a block at the specific <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which block should be deleted.</param>
        /// <returns>This block.</returns>
        /// <exception cref="IndexOutOfRangeException">index is less than 0. -or- index is equal or greater than <see cref="ContentIds"/> Count.</exception>
        public virtual ContentBlock RemoveBlock(int index)
        {
            if (index >= ContentIds.Count || index < 0) throw new IndexOutOfRangeException();

            string blockId = ContentIds[index];
            Dictionary<string, object?> args = new() { { "alive", false } };
            Client.OperationsToTransaction();
            Client.AddOperation(Api.OperationBuilder.MainOperation(Api.MainCommand.update, blockId, "block", Array.Empty<string>(), args));
            Client.AddOperation(
                Api.OperationBuilder.ListRemovingOperation(Id, blockId),
                () =>
                {
                    ContentIds.Remove(blockId);
                    if (Content.Count > index) Content.RemoveAll((block) => block.Id == blockId);
                }
            );
            return this;
        }

        /// <summary>
        /// Creates a new operation that
        /// removes a block with <paramref name="blockId"/>
        /// </summary>
        /// <param name="blockId">ID of child block to be removed.</param>
        /// <returns>This block.</returns>
        /// <exception cref="ArgumentException">Block with <paramref name="blockId"/> isn't a child of current block.</exception>
        public virtual ContentBlock RemoveBlock(string blockId)
        {
            blockId = Utils.ExtractId(blockId);
            int blockIndex = ContentIds.IndexOf(blockId);
            if (blockIndex == -1) throw new ArgumentException("Block with that ID isn't a child of current block", nameof(blockId));
            return RemoveBlock(blockIndex);
        }

        /// <summary>
        /// Creates an operations that
        /// remove blocks at <paramref name="indexes"/>.
        /// </summary>
        /// <param name="indexes">The zero-based indexes at which blocks should be removed.</param>
        /// <inheritdoc cref="RemoveBlock(int)"/>
        public virtual ContentBlock RemoveBlocks(IEnumerable<int> indexes)
        {
            foreach (int index in indexes) RemoveBlock(index);
            return this;
        }

        /// <summary>
        /// Creates an operations that
        /// remove blocks with <paramref name="blockIds"/>.
        /// </summary>
        /// <param name="blockIds">IDs of child blocks to be removed.</param>
        /// <inheritdoc cref="RemoveBlock(string)"/>
        public virtual ContentBlock RemoveBlocks(IEnumerable<string> blockIds)
        {
            foreach (string id in blockIds) RemoveBlock(id);
            return this;
        }
    }

    public abstract class TitleContentBlock<T> : ContentBlock, ITitleBlock<T> where T : BaseBlock
    {
        public string Title { get; protected set; }

        protected TitleContentBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Title = Utils.RecieveTitle(blockValue);
        }

        /// <summary>
        /// Creates a new operation that
        /// sets a new block title
        /// </summary>
        /// <param name="title">New title</param>
        /// <returns>This block</returns>
        public abstract T SetTitle(string title);

        public override string ToString()
        {
            return $"<{GetType().Name}, {Id}, Title: \"{Title}\", Content count: {ContentIds.Count}>";
        }
    }

    public abstract class ColorTitleContentBlock<T> : TitleContentBlock<T>, IColorBlock<T> where T : BaseBlock
    {
        public BlockColor Color { get; set; }

        protected ColorTitleContentBlock(Client client, RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

        public abstract T SetColor(BlockColor color);
    }
}