using Newtonsoft.Json;

namespace Systems.WorldSystem
{
    using System;
    
    public class WorldGrid<T>
    {
        [JsonProperty]
        public int Width { get; private set; }
        
        [JsonProperty]
        public int Height { get; private set;}
        
        [JsonProperty]
        private T[] Data { get; set; }
        public WorldGrid(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Grid dimensions must be positive.");

            Width = width;
            Height = height;
            Data = new T[width * height];
        }

        [JsonConstructor]
        private WorldGrid(int width, int height, T[] data)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Grid dimensions must be positive.");
            Width = width;
            Height = height;
            Data = data;
        }

        public T this[int x, int y]
        {
            get => Data[ToIndex(x, y)];
            set => Data[ToIndex(x, y)] = value;
        }
        

        public T this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        public bool TryGet(int x, int y, out T value)
        {
            if (IsInBounds(x, y))
            {
                value = Data[ToIndex(x, y)];
                return true;
            }
            
            value = default(T);
            return false;
        }

        public int ToIndex(int x, int y) => y * Width + x;

        public (int x, int y) ToCoords(int index) 
            => (index % Width, index / Width);
        
        public bool IsInBounds (int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

        public void Clear() => Array.Clear(Data, 0, Data.Length);
        
        
    }
    
}