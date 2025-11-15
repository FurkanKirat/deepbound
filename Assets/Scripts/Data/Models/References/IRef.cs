namespace Data.Models.References
{
    public interface IRef<out T>
    {
        string Key { get; }
        T Load();
    }

}