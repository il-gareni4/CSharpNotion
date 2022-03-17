namespace CSharpNotion.Entities.Interfaces
{
    public interface ICaptionBlock<T> where T : BaseBlock
    {
        string Caption { get; }

        T SetCaption(string caption);
    }
}