using System.Collections.Generic;

namespace Utils
{
    public static class ShapeHelper
    {
        /// <summary>
        /// Returns all integer coordinates inside an ellipse.
        /// </summary>
        public static IEnumerable<(int x, int y)> GetEllipse(int centerX, int centerY, int radiusX, int radiusY)
        {
            for (int dx = -radiusX; dx <= radiusX; dx++)
            {
                for (int dy = -radiusY; dy <= radiusY; dy++)
                {
                    float nx = dx / (float)radiusX;
                    float ny = dy / (float)radiusY;

                    if (nx * nx + ny * ny <= 1.0f)
                        yield return (centerX + dx, centerY + dy);
                }
            }
        }

        /// <summary>
        /// Returns all integer coordinates inside a circle.
        /// </summary>
        public static IEnumerable<(int x, int y)> GetCircle(int centerX, int centerY, int radius)
            => GetEllipse(centerX, centerY, radius, radius);

        /// <summary>
        /// Returns all integer coordinates inside a rectangle.
        /// </summary>
        public static IEnumerable<(int x, int y)> GetRectangle(int centerX, int centerY, int width, int height)
        {
            var startX = centerX - width / 2;
            var startY = centerY - height / 2;
            for (int dx = 0; dx <= width; dx++)
            {
                for (int dy = 0; dy <= height; dy++)
                {
                    var x = dx + startX;
                    var y = dy + startY;
                    
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int x, int y)> GetRectangleEdges(int centerX, int centerY, int width, int height)
        {
            var startX = centerX - width / 2;
            var startY = centerY - height / 2;
            for (int x = 0; x <= width; x++)
            {
                yield return (startX + x, startY);
                yield return (startX + x, startY + height);
            }

            for (int y = 1; y <= height - 1; y++)
            {
                yield return (startX, startY + y);
                yield return (startX + width - 1, startY + y);
            }
        }

        /// <summary>
        /// Returns all integer coordinates inside a square.
        /// </summary>
        public static IEnumerable<(int x, int y)> GetSquare(int centerX, int centerY, int sideLength)
            => GetRectangle(centerX, centerY, sideLength, sideLength);

    }
}