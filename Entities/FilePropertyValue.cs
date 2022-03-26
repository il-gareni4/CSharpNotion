using CSharpNotion.Api.Response;

namespace CSharpNotion.Entities
{
    public class FilePropertyValue
    {
        public string Filename { get; set; }
        public string Url { get; set; }

        public FilePropertyValue(string filename, string url)
        {
            Filename = filename;
            Url = url;
        }

        public static async Task<FilePropertyValue> UploadFileAndCreateNew(Client client, string collectionRowPageId, string filePath)
        {
            FileInfo fileInfo = new(filePath);
            GetUploadFileUrlResponse urls = await Api.ApiMaster.UploadFile(client, collectionRowPageId, filePath);
            return new FilePropertyValue(fileInfo.Name, urls.Url!);
        }
    }
}