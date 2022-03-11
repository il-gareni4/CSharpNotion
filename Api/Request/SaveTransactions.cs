namespace CSharpNotion.Api.Request
{
    public class SaveTransactions
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public Transaction[]? Transactions { get; set; }
    }

    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Operation[]? Operations { get; set; }
    }

    public class Operation
    {
        public Dictionary<string, object?>? Args { get; set; }
        public string? Command { get; set; }
        public string[]? Path { get; set; }
        public General.Pointer? Pointer { get; set; }
    }
}