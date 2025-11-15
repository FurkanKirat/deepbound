namespace Interfaces
{
    public interface IInitializable<in T>
    {
        public void Initialize(T data);
    }

    public interface IInitializable
    {
        public void Initialize();
    }
}