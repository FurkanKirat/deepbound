using UnityEngine;

namespace Visuals.UI
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] public Texture2D[] cursorTextures;
        [SerializeField] public int cursorTextureIndex;

        public void Initialize()
        {
            if (cursorTextureIndex >= cursorTextures.Length)
                cursorTextureIndex = 0;
            Cursor.SetCursor(cursorTextures[cursorTextureIndex], Vector2.zero, CursorMode.Auto);
        }
    }
}