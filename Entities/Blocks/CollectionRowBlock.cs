using CSharpNotion.Api.General;
using CSharpNotion.Entities.CollectionProperties;
using System.Text.Json;

namespace CSharpNotion.Entities
{
    public class CollectionRowBlock : PageBlock
    {
        internal Collection ParentCollection { get; set; }
        internal Dictionary<string, JsonElement[][]> Properties { get; set; }

        internal CollectionRowBlock(Client client, RecordMapBlockValue blockValue, Collection collection) : base(client, blockValue)
        {
            ParentCollection = collection;
            Properties = blockValue.Properties ?? new Dictionary<string, JsonElement[][]>();
        }

        private JsonElement[][]? GetPropertyValue<T>(string propertyName) where T : BaseProperty
        {
            BaseProperty? prop = ParentCollection!.GetPropertyByName(propertyName);
            if (prop is null) return null;
            if (prop is not T) throw new ArgumentException($"This property is not {typeof(T).Name}", nameof(T));

            return Properties!.GetValueOrDefault(prop.Id);
        }

        private T GetProperty<T>(string propertyName) where T : BaseProperty
        {
            BaseProperty? prop = ParentCollection!.GetPropertyByName(propertyName);
            if (prop is null) throw new ArgumentException($"Property with this name doesn't exist", nameof(propertyName));
            if (prop is not T) throw new ArgumentException($"This property is not {typeof(T).Name}", nameof(T));

            return (T)prop;
        }

        private void SetProperty(string propertyId, IEnumerable<object>? propertyValue, bool wrapOuterArray = true)
        {
            JsonElement[][]? resultPropValue = null;
            if (propertyValue is not null)
            {
                if (wrapOuterArray) resultPropValue = new JsonElement[][] {
                    propertyValue.Select((val) => JsonSerializer.SerializeToElement(val, Client.SerializeOptions)).ToArray()
                };
                else resultPropValue = propertyValue.Select(
                    (val) => ((IEnumerable<object>)val).Select(
                        (val2) => JsonSerializer.SerializeToElement(val2, Client.SerializeOptions)).ToArray()
                    ).ToArray();
            }
            Dictionary<string, object?> args = new() { { propertyId, resultPropValue } };
            Client.AddOperation(
                Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args),
                () =>
                {
                    if (resultPropValue is null) Properties.Remove(propertyId);
                    else Properties[propertyId] = resultPropValue;
                }
            );
        }

        private string? GetTextProperty<T>(string propertyName) where T : BaseProperty
        {
            JsonElement[][]? propValue = GetPropertyValue<T>(propertyName);
            if (propValue is null) return null;

            return propValue[0][0].GetString();
        }

        public string? GetTextProperty(string propertyName) => GetTextProperty<TextProperty>(propertyName);

        public double? GetNumberProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetPropertyValue<NumberProperty>(propertyName);
            if (propValue is null) return null;

            return propValue[0][0].GetDouble();
        }

        public string[]? GetMultiselectProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetPropertyValue<MultiselectProperty>(propertyName);
            if (propValue is null) return null;

            return propValue[0][0].GetString()!.Split(",");
        }

        public string? GetUrlProperty(string propertyName) => GetTextProperty<UrlProperty>(propertyName);

        public string? GetSelectProperty(string propertyName) => GetTextProperty<SelectProperty>(propertyName);

        public string? GetPhoneNumberProperty(string propertyName) => GetTextProperty<PhoneNumberProperty>(propertyName);

        public string? GetEmailProperty(string propertyName) => GetTextProperty<EmailProperty>(propertyName);

        public bool? GetCheckboxProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetPropertyValue<CheckboxProperty>(propertyName);
            if (propValue is null) return null;

            return propValue[0][0].GetString() == "Yes";
        }

        private string[]? GetLinkIdsProperty<T>(string propertyName) where T : BaseProperty
        {
            JsonElement[][]? propValue = GetPropertyValue<T>(propertyName);
            if (propValue is null) return null;

            return propValue[0][1].Deserialize<string[][]>()![0][1..];
        }

        public string[]? GetPersonProperty(string propertyName) => GetLinkIdsProperty<PersonProperty>(propertyName);

        public string[]? GetRelationProperty(string propertyName) => GetLinkIdsProperty<RelationProperty>(propertyName);

        public FilePropertyValue[]? GetFileProperty(string propertyName)
        {
            JsonElement[][]? propValue = GetPropertyValue<FileProperty>(propertyName);
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
            JsonElement[][]? propValue = GetPropertyValue<DateProperty>(propertyName);
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

        private object[]? WarpToArrayOrNull(object? val) => val is null ? null : new object[] { val };

        public CollectionRowBlock SetTextProperty(string propertyName, string? text)
        {
            TextProperty prop = GetProperty<TextProperty>(propertyName);
            SetProperty(prop.Id, WarpToArrayOrNull(text));
            return this;
        }

        public CollectionRowBlock SetNumberProperty(string propertyName, double? number)
        {
            NumberProperty prop = GetProperty<NumberProperty>(propertyName);
            SetProperty(prop.Id, WarpToArrayOrNull(number));
            return this;
        }

        private CollectionRowBlock SetLinkProperty<T>(string propertyName, string? propertyValue) where T : TextProperty
        {
            T prop = GetProperty<T>(propertyName);
            if (string.IsNullOrEmpty(propertyValue)) SetProperty(prop.Id, null);
            else
            {
                object[] resultPropertyValue = new object[]
                {
                    propertyValue,
                    new string[][]
                    {
                        new string[]
                        {
                            "a",
                            propertyValue
                        }
                    }
                };
                SetProperty(prop.Id, resultPropertyValue);
            }
            return this;
        }

        private CollectionRowBlock SetMultiLinkProperty<T>(string propertyName, string linkType, IEnumerable<string>? propertyValue) where T : BaseProperty
        {
            T prop = GetProperty<T>(propertyName);
            if (propertyValue is null) SetProperty(prop.Id, null);
            else
            {
                object[] resultPropertyValue = new object[]
                {
                    "‣",
                    new string[][]
                    {
                        new string[]
                        {
                            linkType
                        }.Concat(propertyValue).ToArray()
                    }
                };
                SetProperty(prop.Id, resultPropertyValue);
            }
            return this;
        }

        public CollectionRowBlock SetUrlProperty(string propertyName, string? url)
        {
            return SetLinkProperty<UrlProperty>(propertyName, url);
        }

        public CollectionRowBlock SetEmailProperty(string propertyName, string? email)
        {
            return SetLinkProperty<EmailProperty>(propertyName, email);
        }

        public CollectionRowBlock SetPhoneNumberProperty(string propertyName, string? phone)
        {
            return SetLinkProperty<PhoneNumberProperty>(propertyName, phone);
        }

        public CollectionRowBlock SetCheckboxProperty(string propertyName, bool checkedState)
        {
            CheckboxProperty prop = GetProperty<CheckboxProperty>(propertyName);
            SetProperty(prop.Id, WarpToArrayOrNull(checkedState ? "Yes" : "No"));
            return this;
        }

        public CollectionRowBlock SetSelectProperty(string propertyName, string? element)
        {
            SelectProperty prop = GetProperty<SelectProperty>(propertyName);
            if (element is null) SetProperty(prop.Id, null);
            else
            {
                prop.AddNewOption(element, Utils.RandomEnumValue<SelectOptionColor>());
                SetProperty(prop.Id, WarpToArrayOrNull(element));
            }
            return this;
        }

        public CollectionRowBlock SetMultiselectProperty(string propertyName, IEnumerable<string>? elements)
        {
            MultiselectProperty prop = GetProperty<MultiselectProperty>(propertyName);
            if (elements is null) SetProperty(prop.Id, null);
            else
            {
                foreach (string option in elements) prop.AddNewOption(option, Utils.RandomEnumValue<SelectOptionColor>());
                SetProperty(prop.Id, WarpToArrayOrNull(string.Join(",", elements)));
            }
            return this;
        }

        public CollectionRowBlock SetPersonProperty(string propertyName, IEnumerable<string>? userIds)
        {
            return SetMultiLinkProperty<PersonProperty>(propertyName, "u", userIds?.Select((id) => Utils.ExtractId(id)));
        }

        public CollectionRowBlock SetRelationProperty(string propertyName, IEnumerable<string>? pageIds)
        {
            return SetMultiLinkProperty<RelationProperty>(propertyName, "p", pageIds?.Select((id) => Utils.ExtractId(id)));
        }

        public CollectionRowBlock SetFileProperty(string propertyName, IEnumerable<FilePropertyValue>? files)
        {
            FileProperty prop = GetProperty<FileProperty>(propertyName);
            if (files is null) SetProperty(prop.Id, null);
            else
            {
                List<object> resultPropValue = new();
                foreach (FilePropertyValue file in files)
                {
                    if (resultPropValue.Count > 0) resultPropValue.Add(new string[] { "," });
                    resultPropValue.Add(new object[] { file.Filename, new string[][] { new string[] { "a", file.Url } } });
                }
                SetProperty(prop.Id, resultPropValue, false);
            }
            return this;
        }

        public CollectionRowBlock SetDateProperty(string propertyName, DatePropertyValue? dateValue)
        {
            DateProperty prop = GetProperty<DateProperty>(propertyName);
            if (dateValue is null) SetProperty(prop.Id, null);
            else
            {
                object[] resultPropertyValue = new object[]
                {
                    "‣",
                    new object[][]
                    {
                        new object[]
                        {
                            "d",
                            dateValue.ToBlockDateInformation()
                        }
                    }
                };
                SetProperty(prop.Id, resultPropertyValue);
            }
            return this;
        }

        public override CollectionRowBlock SetTitle(string title)
        {
            base.SetTitle(title);
            return this;
        }
    }
}