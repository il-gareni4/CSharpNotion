using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class ImageBlock : FileContainingBlock
    {
        public ImageBlock(RecordMapBlockValue blockValue) : base(blockValue)
        { }

        public async Task SetImageUrl(string imageUrl, bool setDisplaySource = true) => await base.SetFileUrl(imageUrl, setDisplaySource);

        public async Task UploadAndSetImage(string filePath) => await base.UploadAndSetFile(filePath);
    }
}