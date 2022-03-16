namespace CSharpNotion.Api.Request
{
    internal class SaveTransactions
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public Transaction[]? Transactions { get; set; }
    }

    internal class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Operation[]? Operations { get; set; }
    }

    internal class Operation
    {
        public Dictionary<string, object?>? Args { get; set; }
        public string? Command { get; set; }
        public string[]? Path { get; set; }
        public General.Pointer? Pointer { get; set; }
    }
}