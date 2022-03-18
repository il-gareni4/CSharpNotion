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
    }
}
