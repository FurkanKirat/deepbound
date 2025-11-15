namespace Systems.EntitySystem
{
    public class EntityUtils
    {
        public static bool IsEmpty(string id)
            => string.IsNullOrEmpty(id) || id == "empty";
    }
}