using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class VideoBlock : FileContainingBlock
    {
        public VideoBlock(RecordMapBlockValue blockValue) : base(blockValue)
        { }

        public async Task SetVideoUrl(string videoUrl) => await base.BaseSetFileUrl(videoUrl);

        public async Task UploadAndSetVideo(string videoPath, string? fileName) => await base.BaseUploadAndSetFile(videoPath, fileName);
    }
}
