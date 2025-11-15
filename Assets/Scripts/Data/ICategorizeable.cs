namespace Data
{
    public interface ICategorizeable<out T>
    {
        public T Category { get; }
    }
}