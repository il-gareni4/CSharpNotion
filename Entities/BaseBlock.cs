using System.Text.Json;

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

        protected virtual Api.General.Pointer SelfPointer => new(Id, "block");

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

    public interface ITitleBlock
    {
        string Title { get; }

        Task SetTitle(string newTitle);
    }

    public enum BlockColor
    {
        Default,
        Gray,
        Brown,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple,
        Pink,
        Red,
        GrayBackground,
        BrownBackground,
        OrangeBackground,
        YellowBackground,
        GreenBackground,
        BlueBackground,
        PurpleBackground,
        PinkBackground,
        RedBackground
    }

    public static class BlockColorExtensions
    {
        public static string? ToColorString(this BlockColor blockColor)
        {
            if (blockColor == BlockColor.Default) return null;
            else if (blockColor == BlockColor.Green) return "teal";
            else if (blockColor == BlockColor.GreenBackground) return "teal_background";

            string stringBlockColor = blockColor.ToString();
            if (stringBlockColor.Contains("Background")) return stringBlockColor[..^10].ToLower() + "_" + stringBlockColor[^10..].ToLower();
            else return stringBlockColor.ToLower();
        }

        public static BlockColor ToBlockColor(string? stringBlockColor)
        {
            if (string.IsNullOrEmpty(stringBlockColor)) return BlockColor.Default;
            else if (stringBlockColor == "teal") return BlockColor.Green;
            else if (stringBlockColor == "teal_background") return BlockColor.GreenBackground;

            string TitleCaseStringBlockColor = string.Join("", stringBlockColor.Split('_').Select((word) => char.ToUpper(word[0]) + word[1..]));
            return Enum.Parse<BlockColor>(TitleCaseStringBlockColor);
        }
    }

    public interface IColorBlock
    {
        BlockColor Color { get; }

        Task SetColor(BlockColor color);
    }

    public abstract class TitleContainingBlock : BaseBlock, ITitleBlock
    {
        public string Title { get; protected set; }

        protected TitleContainingBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        {
            Title = blockValue?.Properties?.Title?.ElementAt(0)[0].GetString() ?? "";
        }

        public virtual async Task SetTitle(string newTitle)
        {
            try
            {
                Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { newTitle } } } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Title = newTitle;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }

    public abstract class ContentBlock : BaseBlock
    {
        protected List<BaseBlock> Content { get; set; } = new List<BaseBlock>();

        public List<string> ContentIds { get; protected set; }

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

    public abstract class TitleContentBlock : ContentBlock, ITitleBlock
    {
        public string Title { get; protected set; }

        protected TitleContentBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        {
            Title = blockValue?.Properties?.Title?.ElementAt(0)[0].GetString() ?? "";
        }

        public virtual async Task SetTitle(string newTitle)
        {
            try
            {
                Dictionary<string, object?> args = new() { { "title", new string[][] { new string[] { newTitle } } } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Title = newTitle;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }

    public abstract class ColorTitleContentBlock : TitleContentBlock, IColorBlock
    {
        public BlockColor Color { get; set; }

        protected ColorTitleContentBlock(Api.Response.RecordMapBlockValue blockValue) : base(blockValue)
        {
            Color = BlockColorExtensions.ToBlockColor(blockValue?.Format?.BlockColor);
        }

        public async Task SetColor(BlockColor color)
        {
            if (color == Color) return;
            try
            {
                Dictionary<string, object?> args = new() { { "block_color", color.ToColorString() } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "format" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Color = color;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}