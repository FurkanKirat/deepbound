using Interfaces;

namespace Data.Database
{
    public interface IMapDatabase<T> : IReferenceSource<T>, IDatabase
    {
        
    }
}