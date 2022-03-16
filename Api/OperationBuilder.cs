using CSharpNotion.Api.Request;
using CSharpNotion.Api.Response;

namespace CSharpNotion.Api
{
    public enum ListCommand
    {
        listAfter,
        listBefore
    }

    public enum MainCommand
    {
        set,
        update
    }

    internal static class OperationBuilder
    {
        public static Operation ListInsertingOperation(ListCommand command, string parentId, string childId, string? beforeAfterId = null)
        {
            Dictionary<string, object?> args = new();
            args["id"] = childId;
            if (!string.IsNullOrEmpty(beforeAfterId))
                args[command.ToString()[4..].ToLower()] = beforeAfterId;
            return new Operation()
            {
                Args = args,
                Command = command.ToString(),
                Path = new string[1] { "content" },
                Pointer = new General.Pointer(parentId, "block")
            };
        }

        public static Operation ListRemovingOperation(string parentId, string childId)
        {
            Dictionary<string, object?> args = new() { { "id", childId } };
            return new Operation()
            {
                Args = args,
                Command = "listRemove",
                Path = new string[1] { "content" },
                Pointer = new General.Pointer(parentId, "block")
            };
        }

        public static Operation FromBlockValueToSetOperation(RecordMapBlockValue blockValue)
        {
            Dictionary<string, object?> args = new();
            foreach (System.Reflection.PropertyInfo propertyInfo in typeof(RecordMapBlockValue).GetProperties())
            {
                object? value = propertyInfo.GetValue(blockValue);
                string snakeCasePropertyName = string.Concat(propertyInfo.Name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
                if (value is not null) args[snakeCasePropertyName] = value;
            }
            return new Operation()
            {
                Args = args,
                Command = "set",
                Path = Array.Empty<string>(),
                Pointer = new General.Pointer(blockValue.Id!, "block")
            };
        }

        public static Operation MainOperation(MainCommand command, string blockId, string table, string[] path, Dictionary<string, object?> args) => new Operation
        {
            Command = command.ToString(),
            Path = path,
            Pointer = new General.Pointer(blockId, table),
            Args = args
        };
    }
}