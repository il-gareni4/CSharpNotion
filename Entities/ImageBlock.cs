using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class ImageBlock : FileContainingBlock
    {
        public ImageBlock(RecordMapBlockValue blockValue) : base(blockValue)
        { }

        public async Task SetImageUrl(string imageUrl) => await base.BaseSetFileUrl(imageUrl);

        public async Task UploadAndSetImage(string imagePath, string? fileName = null) => await base.BaseUploadAndSetFile(imagePath, fileName);
    }
}