using CSharpNotion.Api.General;
using CSharpNotion.Entities;
using CSharpNotion.Entities.Blocks;
using CSharpNotion.Entities.CollectionProperties;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace CSharpNotion
{
    internal static class Utils
    {
        private static Random _random = new();

        public static string ExtractId(string id)
        {
            if (!id.Contains('-'))
                return id[..8] + "-" + id[8..12] + "-" + id[12..16] + "-" + id[16..20] + "-" + id[20..];
            return id;
        }

        public static void SetHttpContent(ref HttpRequestMessage httpRequest, object data)
        {
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(data, Client.SerializeOptions));
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        public static BaseBlock ConvertBlockFromResponse(Client client, RecordMapBlockValue blockValue)
        {
            if (blockValue.Type is null) throw new ArgumentNullException("blockValue.Type can't be null");
            Type blockType = Constants.TypeNameToBlockType.GetValueOrDefault(blockValue.Type, typeof(DividerBlock));
            return (BaseBlock)Activator.CreateInstance(blockType,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null, new object[] { client, blockValue }, null)!;
        }

        public static string RecieveTitle(RecordMapBlockValue blockValue) =>
            blockValue?.Properties?.GetValueOrDefault("title")?.ElementAt(0)[0].GetString() ?? "";

        public static RecordMapBlockValue CreateNewBlockValue<T>(Client client, string spaceId, string parentId, string parentTable = "block") where T : BaseBlock
        {
            long createdTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return new RecordMapBlockValue()
            {
                Id = Guid.NewGuid().ToString(),
                SpaceId = spaceId,
                Type = Constants.BlockTypeToTypeName.GetValueOrDefault(typeof(T), "text"),
                Version = 1,
                Alive = true,
                ParentId = parentId,
                ParentTable = parentTable,
                CreatedTime = createdTime,
                LastEditedTime = createdTime,
                CreatedById = client.User.Id,
                LastEditedById = client.User.Id,
                CreatedByTable = "notion_user",
                LastEditedByTable = "notion_user"
            };
        }

        public static T ActivatorCreateNewBlock<T>(Client client, RecordMapBlockValue blockValue)
        {
            return (T)Activator.CreateInstance(
                typeof(T), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null, new object[] { client, blockValue }, null)!;
        }

        public static List<BaseProperty> ConvertSchemaToPropertiesList(Client client, Dictionary<string, CollectionValueSchemaElement> schema, Collection collection)
        {
            List<BaseProperty> list = new();
            foreach (KeyValuePair<string, CollectionValueSchemaElement> property in schema)
            {
                Type propertyType = Constants.TypeNameToPropertyType.GetValueOrDefault(property.Value.Type!, typeof(TextProperty));
                list.Add((BaseProperty)Activator.CreateInstance(propertyType,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null, new object[] { client, property, collection }, null)!);
            }
            return list;
        }

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_random.Next(v.Length))!;
        }

        public static string FormatToNotionTime(int hour, int minute)
        {
            static string formatUnit(int val)
            {
                if (val > 9) return val.ToString();
                else return "0" + val.ToString();
            }
            return $"{formatUnit(hour)}:{formatUnit(minute)}";
        }

        public static string FormatToNotionTime(TimeOnly time) => FormatToNotionTime(time.Hour, time.Minute);

        public static string FormatToNotionTime(DateTime dateTime) => FormatToNotionTime(dateTime.Hour, dateTime.Minute);

        public static string FormatToNotionDate(int year, int month, int day)
        {
            static string formatUnit(int val)
            {
                if (val > 9) return val.ToString();
                else return "0" + val.ToString();
            }
            return $"{year}-{formatUnit(month)}-{formatUnit(day)}";
        }

        public static string FormatToNotionDate(DateTime dateTime) => FormatToNotionDate(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}