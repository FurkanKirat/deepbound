namespace Core
{
    public interface IIdentifiable
    {
        public string Id { get; }
    }

    public static class IdentifiableExtensions
    {
        public static bool IsSameAs(this IIdentifiable i, IIdentifiable j)
        {
            return i!= null && j!=null && i.Id == j.Id;
        }

        public static bool IsSameAs(this IIdentifiable i, string j)
        {
            return i != null && j != null && i.Id == j;
        }
    }

}
