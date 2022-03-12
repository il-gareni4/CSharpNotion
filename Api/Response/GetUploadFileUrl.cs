namespace CSharpNotion.Api.Response
{
    public class GetUploadFileUrlResponse
    {
        public string? SignedGetUrl { get; set; }
        public string? SignedPutUrl { get; set; }
        public string? Url { get; set; }
    }
}