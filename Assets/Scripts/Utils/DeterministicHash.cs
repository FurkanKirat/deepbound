namespace Utils
{
    public static class DeterministicHash
    {
        public static int Fnv1aHash(string text, int seed = 0)
        {
            const uint fnvOffsetBasis = 2166136261;
            const uint fnvPrime = 16777619;

            uint hash = fnvOffsetBasis ^ (uint)seed;
            foreach (char c in text)
            {
                hash ^= c;
                hash *= fnvPrime;
            }

            return unchecked((int)hash);
        }
    }

}