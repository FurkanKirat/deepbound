namespace Utils
{
    using System.IO;
    using UnityEngine;

    public static class ImageLoader
    {
        /// <summary>
        /// Loads a PNG (or JPG) file into a Texture2D.
        /// </summary>
        public static Texture2D LoadTexture(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(fileData);
            return tex;
        }

        /// <summary>
        /// Loads a PNG (or JPG) file and converts it directly into a Sprite.
        /// </summary>
        public static Sprite LoadSprite(string filePath, Vector2? pivot = null, float pixelsPerUnit = 100f)
        {
            Texture2D tex = LoadTexture(filePath);
            if (tex == null)
                return null;

            var rect = new Rect(0, 0, tex.width, tex.height);
            var p = pivot ?? new Vector2(0.5f, 0.5f);
            return Sprite.Create(tex, rect, p, pixelsPerUnit);
        }
    }

}