namespace Data.Serializable
{
    using UnityEngine;

    [System.Serializable]
    public struct FloatRect
    {
        public float x, y, width, height;

        public FloatRect(Rect r)
        {
            x = r.x;
            y = r.y;
            width = r.width;
            height = r.height;
        }

        public Rect ToRect() => new Rect(x, y, width, height);

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}, {nameof(width)}: {width}, {nameof(height)}: {height}";
        }
    }

}