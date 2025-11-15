namespace Data.RegistrySystem
{
    public interface ICreateFactory<in TInput, out TOutput> : IFactory
    {
        public TOutput Create(string id, TInput input);
        
    }
}