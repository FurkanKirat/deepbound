namespace Data.Models.References
{
    public interface IFactoryRef<in TContext, out TResult>
    {
        string Key { get; }
        TResult Create(TContext context);
    }

}