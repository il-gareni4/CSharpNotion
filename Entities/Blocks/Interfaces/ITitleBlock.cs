namespace CSharpNotion.Entities.Blocks.Interfaces
{
    public interface ITitleBlock<T> where T : BaseBlock
    {
        string Title { get; }

        T SetTitle(string title);
    }
}