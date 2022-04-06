using CSharpNotion.Api.General;
using CSharpNotion.Entities.Blocks;
using System.Net.Http.Headers;
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
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(data, Constants.SerializeOptions));
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        public static string RecieveTitle(RecordMapBlockValue blockValue)
        {
            if (blockValue?.Properties?.GetValueOrDefault("title") is null) return "";
            string resultString = "";
            foreach (JsonElement[] part in blockValue.Properties["title"])
            {
                string? str = part[0].GetString();
                if (str != "‣") resultString += str;
            }
            return resultString;
        }

        public static RecordMapBlockValue CreateNewBlockValue<T>(Client client, string spaceId, string parentId, string parentTable = "block") where T : BaseBlock
        {
            long createdTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return new RecordMapBlockValue()
            {
                Id = Guid.NewGuid().ToString(),
                SpaceId = spaceId,
                Type = Constants.NotionBlockTypes.GetValueOrDefault(typeof(T), "text"),
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