namespace CSharpNotion.Entities
{
    public class TodoBlock : ColorTitleContentBlock
    {
        public bool Checked { get; private set; }

        public TodoBlock(Client client, Api.Response.RecordMapBlockValue blockValue) : base(client, blockValue)
        {
            string? checkedValue = blockValue?.Properties?.Checked?.ElementAt(0)[0];
            Checked = checkedValue == "Yes";
        }

        public async Task ToggleChecked() => await SetChecked(!Checked);

        public async Task SetChecked(bool checkedState)
        {
            if (checkedState == Checked) return;
            try
            {
                Dictionary<string, object?> args = new() { { "checked", new string[][] { new string[] { checkedState ? "Yes" : "No" } } } };
                Api.Request.Operation operation = Api.OperationBuilder.MainOperation(Api.MainCommand.update, Id, "block", new string[] { "properties" }, args);
                (await QuickRequestSetup.SaveTransactions(operation).Send(Client.HttpClient)).EnsureSuccessStatusCode();
                Checked = checkedState;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}