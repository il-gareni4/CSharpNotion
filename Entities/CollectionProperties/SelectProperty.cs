using CSharpNotion.Api.General;

namespace CSharpNotion.Entities.CollectionProperties
{
    public class SelectProperty : TextProperty
    {
        public List<SelectPropertyOption> Options { get; set; }

        internal SelectProperty(Client client, KeyValuePair<string, CollectionValueSchemaElement> property, Collection collection) : base(client, property, collection)
        {
            Options = property.Value.Options?.Select((option) => new SelectPropertyOption(option)).ToList() ?? new List<SelectPropertyOption>();
        }

        public SelectProperty AddNewOption(string value, SelectOptionColor color)
        {
            if (Options.Exists((option) => option.Value == value)) return this;
            string newOptionId = Guid.NewGuid().ToString();
            Dictionary<string, object?> args = new()
            {
                { "id", newOptionId },
                { "value", value },
                { "color", color.ToString().ToLower() }
            };
            Client.AddOperation(
                Api.OperationBuilder.KeyedObjectListAfter(Collection.Id, "collection", new string[] { "schema", Id, "options" }, args),
                () => Options.Add(new SelectPropertyOption(newOptionId, value, color))
            );
            return this;
        }
    }

    public enum SelectOptionColor
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
        Red
    }

    public class SelectPropertyOption
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public SelectOptionColor Color { get; set; }

        internal SelectPropertyOption(CollectionSelectOption rawOption)
        {
            Id = rawOption.Id ?? throw new ArgumentNullException();
            Value = rawOption.Value ?? throw new ArgumentNullException();
            string colorName = rawOption.Color ?? throw new ArgumentNullException();
            Color = Enum.Parse<SelectOptionColor>(char.ToUpper(colorName[0]) + colorName[1..]);
        }

        internal SelectPropertyOption(string id, string value, SelectOptionColor color)
        {
            Id = id;
            Value = value;
            Color = color;
        }
    }
}