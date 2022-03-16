namespace CSharpNotion.Api.Request
{
    internal class GetUploadFileUrl
    {
        public string Bucket { get; set; } = "secure";
        public string? ContentType { get; set; }
        public string? Name { get; set; }
        public General.Pointer? Record { get; set; }
    }
}
