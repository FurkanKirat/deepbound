namespace Interfaces
{
    public interface IReferenceSource<T>
    {
        T this[string key] { get; }
        bool TryGet(string key, out T value);
    }
}