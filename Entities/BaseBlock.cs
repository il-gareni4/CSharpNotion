﻿using System.Text.Json;

namespace CSharpNotion.Entities
{
    public abstract class BaseBlock
    {
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

        protected BaseBlock(Api.Response.RecordMapBlockValue blockValue)
        {
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
    }

    public abstract class TitleContainingBlock : BaseBlock
    {
        public string Title { get; protected set; }

        protected TitleContainingBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        {
            Title = blockValue?.Properties?.Title?.ElementAt(0)[0].Deserialize<string>() ?? "";
        }

        public virtual async Task SetTitle(string newTitle)
        {
            try
            {
                Api.Request.Operation operation = new()
                {
                    Args = new Dictionary<string, object?> {
                        {"title", new string[][] { new string[] { newTitle } } }
                    },
                    Command = "set",
                    Path = new string[1] { "properties" },
                    Pointer = new Api.General.Pointer(Id, "block")
                };
                await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient);
                Title = newTitle;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }

    public abstract class ContentBlock : TitleContainingBlock
    {
        protected List<BaseBlock> Content { get; set; } = new List<BaseBlock>();

        protected List<string> ContentIds { get; set; }

        protected ContentBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
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

                Content = syncResponse.RecordMap.Block.Select((pair) => Utils.ConvertBlockFromResponse(pair.Value.Value!)).ToList();
            }
            return Content;
        }

        public virtual async Task<T> AppendBlock<T>(string title = "") where T : BaseBlock
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
                LastEditedTime = createdTime,
                Properties = new Api.Response.RecordMapBlockProperties()
                {
                    Title = new JsonElement[][] { new JsonElement[] { JsonSerializer.SerializeToElement(title) } }
                }
            };
            Api.Request.Operation[] operations = new Api.Request.Operation[]
            {
                Api.OperationBuilder.FromBlockValueToSetOperation(newBlock),
                Api.OperationBuilder.ListOperation(Api.ListCommand.listAfter, Id, newBlock.Id, ContentIds.LastOrDefault())
            };
            (await QuickRequestSetup.SaveTransactions(operations).Send(Client.HttpClient)).EnsureSuccessStatusCode();
            T newBlockInstance = (T)Activator.CreateInstance(typeof(T), newBlock)!;
            ContentIds.Add(newBlock.Id);
            Content.Add(newBlockInstance);
            return newBlockInstance;
        }
    }
}