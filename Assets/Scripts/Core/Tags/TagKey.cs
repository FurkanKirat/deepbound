namespace Core.Tags
{
    public sealed class TagKey<T>
    {
        public string Key { get; }

        public TagKey(string key)
        {
            Key = key;
        }

        public override bool Equals(object obj) => obj is TagKey<T> other && Key == other.Key;
        public override int GetHashCode() => Key.GetHashCode();
        public override string ToString() => Key;
    }

}