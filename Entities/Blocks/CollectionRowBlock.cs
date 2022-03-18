using CSharpNotion.Api.General;
using CSharpNotion.Entities.CollectionProperties;
using System.Text.Json;

namespace CSharpNotion.Entities
{
    public class CollectionRowBlock : PageBlock
    {
        internal Collection ParentCollection { get; set; }

        internal CollectionRowBlock(Client client, RecordMapBlockValue blockValue, Collection collection) : base(client, blockValue)
        {
            ParentCollection = collection;
        }

        private JsonElement[][]? GetProperty(string propertyName)
        {
            if (ParentTable != "collection") throw new InvalidOperationException("ParentTable must to be 'collection'");
            BaseProperty? prop = ParentCollection!.GetPropertyByName(propertyName);
            if (prop is null) return null;

            return Properties!.GetValueOrDefault(prop.Id);
        }

        public string? GetTextProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetProperty(propertyName);
            if (propValue is null) return null;

            return propValue[0][0].GetString();
        }

        public double? GetNumberProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetProperty(propertyName);
            if (propValue is null) return null;

            return propValue[0][0].GetDouble();
        }

        public string[]? GetMultiselectProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetProperty(propertyName);
            if (propValue is null) return null;

            return propValue[0][0].GetString()!.Split(",");
        }

        public string? GetLinkProperty(string propertyName) => GetTextProperty(propertyName);

        public string? GetSelectProperty(string propertyName) => GetTextProperty(propertyName);

        public string? GetPhoneNumberProperty(string propertyName) => GetTextProperty(propertyName);

        public string? GetEmailProperty(string propertyName) => GetTextProperty(propertyName);

        public bool? GetCheckboxProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetProperty(propertyName);
            if (propValue is null) return null;

            return propValue[0][0].GetString() == "Yes";
        }

        public string[]? GetPersonProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetProperty(propertyName);
            if (propValue is null) return null;

            return propValue[0][1].Deserialize<string[][]>()![0][1..];
        }

        public string[]? GetRelationProperty(string propertyName) => GetPersonProperty(propertyName);

        public FilePropertyValue[]? GetFileProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetProperty(propertyName);
            if (propValue is null) return null;

            List<FilePropertyValue> files = new();
            for (int i = 0; i < propValue.Length; i += 2)
            {
                string filename = propValue[i][0].GetString()!;
                string fileUrl = propValue[i][1].Deserialize<string[][]>()![0][1];
                files.Add(new FilePropertyValue(filename, fileUrl));
            }
            return files.ToArray();
        }

        public DatePropertyValue? GetDateProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetProperty(propertyName);
            if (propValue is null) return null;

            BlockDateInformation rawDateValue = propValue[0][1]
                .Deserialize<JsonElement[][]>(Client.SerializeOptions)![0][1]
                .Deserialize<BlockDateInformation>(Client.SerializeOptions)!;
            DatePropertyValue resultDateValue;

            bool haveTime = rawDateValue.Type == "datetime" || rawDateValue.Type == "datetimerange";
            if (haveTime)
            {
                DateTime startDate = DateTime.Parse(rawDateValue.StartDate!).Add(TimeSpan.Parse(rawDateValue.StartTime!));
                if (rawDateValue.EndDate is not null)
                {
                    DateTime endDate = DateTime.Parse(rawDateValue.EndDate!).Add(TimeSpan.Parse(rawDateValue.EndTime!));
                    resultDateValue = new DatePropertyValue(startDate, endDate);
                }
                else resultDateValue = new DatePropertyValue(startDate);
            }
            else
            {
                DateOnly startDate = DateOnly.Parse(rawDateValue.StartDate!);
                if (rawDateValue.EndDate is not null)
                {
                    DateOnly endDate = DateOnly.Parse(rawDateValue.EndDate!);
                    resultDateValue = new DatePropertyValue(startDate, endDate);
                }
                else resultDateValue = new DatePropertyValue(startDate);
            }
            if (rawDateValue.Reminder is not null)
            {
                DatePropertyValueReminder reminder = new(rawDateValue.Reminder.Unit!, rawDateValue.Reminder.Value!,
                    rawDateValue.Reminder.Time is not null ? TimeOnly.Parse(rawDateValue.Reminder.Time) : null);
                resultDateValue.Reminder = reminder;
            }
            if (rawDateValue.TimeZone is not null) resultDateValue.TimeZone = rawDateValue.TimeZone;
            return resultDateValue;
        }
    }
}