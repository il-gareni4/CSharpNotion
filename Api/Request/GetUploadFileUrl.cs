namespace CSharpNotion.Api.Request
{
    public class GetUploadFileUrl
    {
        public string Bucket { get; set; } = "secure";
        public string? ContentType { get; set; }
        public string? Name { get; set; }
        public General.Pointer? Record { get; set; }
    }
}
