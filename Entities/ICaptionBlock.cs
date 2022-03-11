namespace CSharpNotion.Entities
{
    public interface ICaptionBlock
    {
        string Caption { get; }
        Task SetCaption(string newCaption);
    }
}
