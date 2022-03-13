using CSharpNotion.Entities.Interfaces;
using System.Text.Json;

namespace CSharpNotion.Entities
{
    public abstract class BaseBlock
    {
        protected Client Client { get; set; }
        public string Id { get; init; }
        public int Version { get; protected set; }
        public string Type { get; protected set; }
        public Api.Response.RecordMapBlockPermission[]? Permissions { get; protected set; }
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

        protected virtual Api.General.Pointer SelfPointer => new(Id, "block");

        protected BaseBlock(Client client, Api.Response.RecordMapBlockValue blockValue)
        {
            Client = client;
            Type = blockValue.Type ?? throw new ArgumentNullException();
            Id = blockValue.Id ?? throw new ArgumentNullException();
            Alive = blockValue.Alive;
            ParentId = blockValue.ParentId ?? throw new ArgumentNullException();
            ParentTable = blockValue.ParentTable ?? throw new ArgumentNullException();
            SpaceId = blockValue.SpaceId ?? throw new ArgumentNullException();

            Version = blockValue.Version;
            Permissions = blockValue?.Permissions;
            CreatedTime = blockValue?.CreatedTime is not null ? DateTimeOffset.FromUnixTimeMilliseconds(blockValue.CreatedTime).DateTime : null;
            LastEditedTime = blockValue?.LastEditedTime is not null ? DateTimeOffset.FromUnixTimeMilliseconds(blockValue.LastEditedTime).DateTime : null;
            CreatedByTable = blockValue?.CreatedByTable;
            CreatedById = blockValue?.CreatedById;
            LastEditedByTable = blockValue?.LastEditedByTable;
            LastEditedById = blockValue?.LastEditedById;
        }

        public virtual async Task Commit() => await Client.Commit();
    }

    public abstract class TitleContainingBlock<T> : BaseBlock, ITitleBlock<T> where T : BaseBlock
    {
        public string Title { get; protected set; }

        protected TitleContainingBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Title = Utils.RecieveTitle(blockValue);
        }

        public abstract T SetTitle(string title);
    }

    public abstract class ContentBlock : BaseBlock
    {
        protected List<BaseBlock> Content { get; set; } = new List<BaseBlock>();

        public List<string> ContentIds { get; protected set; }

        protected ContentBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            ContentIds = blockValue.Content?.ToList() ?? new List<string>();
        }

        public virtual async Task<List<BaseBlock>> GetContent()
        {
            if (ContentIds.Count != Content.Count)
            {
                Api.General.Pointer[] pointers = ContentIds.Select((id) => new Api.General.Pointer(id, "block")).ToArray();
                Api.Response.SyncRecordValuesResponse syncResponse = await QuickRequestSetup.SyncRecordValues(pointers)
                    .Send(Client.HttpClient)
                    .DeserializeJson<Api.Response.SyncRecordValuesResponse>();
                if (syncResponse?.RecordMap?.Block is null) throw new InvalidDataException("Invalid response");

                Content = syncResponse.RecordMap.Block.Select((pair) => Utils.ConvertBlockFromResponse(Client, pair.Value.Value!)).ToList();
            }
            return Content;
        }

        public virtual T AppendBlock<T>() where T : BaseBlock
        {
            long createdTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            Api.Response.RecordMapBlockValue newBlock = new()
            {
                Id = Guid.NewGuid().ToString(),
                SpaceId = SpaceId,
                Type = Constants.BlockToType.GetValueOrDefault(typeof(T), "text"),
                Version = 1,
                Alive = true,
                ParentId = Id,
                ParentTable = "block",
                CreatedTime = createdTime,
                LastEditedTime = createdTime
            };
            Client.AddOperation(Api.OperationBuilder.FromBlockValueToSetOperation(newBlock));
            T newBlockInstance = (T)Activator.CreateInstance(typeof(T), Client, newBlock)!;
            Client.AddOperation(
                Api.OperationBuilder.ListOperation(Api.ListCommand.listAfter, Id, newBlock.Id, ContentIds.LastOrDefault()),
                () =>
                {
                    ContentIds.Add(newBlock.Id);
                    Content.Add(newBlockInstance);
                }
            );
            return newBlockInstance;
        }
    }

    public abstract class TitleContentBlock<T> : ContentBlock, ITitleBlock<T> where T : BaseBlock
    {
        public string Title { get; protected set; }

        protected TitleContentBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Title = Utils.RecieveTitle(blockValue);
        }

        public abstract T SetTitle(string title);
    }

    public abstract class ColorTitleContentBlock<T> : TitleContentBlock<T>, IColorBlock<T> where T : BaseBlock
    {
        public BlockColor Color { get; set; }

        protected ColorTitleContentBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

        public abstract T SetColor(BlockColor color);
    }
}