using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.Tools
{
    public class BlockTextureGeneratorWindow : EditorWindow
    {
        private Color[] _palette = 
        {
            new Color(1f, 1f, 1f),             // Snow White #FFFFFF
            new Color(0.949f, 0.961f, 0.969f), // Soft Snow #F2F5F7
            new Color(0.863f, 0.890f, 0.910f), // Silver Frost #DCE3E8
            new Color(0.690f, 0.718f, 0.757f), // Winter Gray #B0B8C1
            new Color(0.631f, 0.667f, 0.702f)  // Shadow Snow #A1AAB3
        };
        private int _size = 16;
        private float _noiseScale = 5f;
        private float _previewScale = 8f;
        private Texture2D _previewTexture;
        private System.Random _rng = new System.Random();
        private int _currentSeed;

        [MenuItem("KankanGames/Tools/Block Texture Generator")]
        public static void ShowWindow()
        {
            GetWindow<BlockTextureGeneratorWindow>("Block Texture Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Ice/Snow Block Texture Generator", EditorStyles.boldLabel);

            // Palette edit
            int paletteSize = Mathf.Max(1, EditorGUILayout.IntField("Palette Size", _palette.Length));
            if (paletteSize != _palette.Length)
            {
                System.Array.Resize(ref _palette, paletteSize);
            }

            for (int i = 0; i < _palette.Length; i++)
            {
                _palette[i] = EditorGUILayout.ColorField($"Color {i + 1}", _palette[i]);
            }

            _size = EditorGUILayout.IntField("Texture Size", _size);
            _noiseScale = EditorGUILayout.Slider("Noise Scale", _noiseScale, 1f, 20f);
            _currentSeed = EditorGUILayout.IntField("Seed", _currentSeed);
            GUILayout.Space(10);

            if (GUILayout.Button("Generate Texture"))
            {
                _previewTexture = GenerateBlockTextureWithNoise(_currentSeed);
            }

            if (_previewTexture != null)
            {
                GUILayout.BeginHorizontal();
                
                if (GUILayout.Button("Save Texture"))
                {
                    SaveTexture(_previewTexture);
                }

                GUILayout.EndHorizontal();

                _previewScale = EditorGUILayout.Slider("Preview Scale", _previewScale, 1f, 32f);

                int scaleFactor = Mathf.RoundToInt(_previewScale);
                Texture2D upscaled = Upscale(_previewTexture, scaleFactor);
                GUILayout.Label(upscaled);
            }
        }
        
        private Texture2D GenerateBlockTextureWithNoise(int? seed = null)
        {
            if (seed.HasValue)
            {
                _currentSeed = seed.Value;
                _rng = new System.Random(_currentSeed);
            }
            else
            {
                _currentSeed = _rng.Next();
                _rng = new System.Random(_currentSeed);
            }

            Vector2 noiseOffset = new Vector2(
                (float)_rng.NextDouble() * 1000f,
                (float)_rng.NextDouble() * 1000f
            );

            Texture2D texture = new Texture2D(_size, _size)
            {
                filterMode = FilterMode.Point
            };

            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    float nx = (x / (float)_size) * _noiseScale + noiseOffset.x;
                    float ny = (y / (float)_size) * _noiseScale + noiseOffset.y;

                    float noise = Mathf.PerlinNoise(nx, ny);

                    int colorIndex = Mathf.FloorToInt(noise * _palette.Length);
                    colorIndex = Mathf.Clamp(colorIndex, 0, _palette.Length - 1);

                    texture.SetPixel(x, y, _palette[colorIndex]);
                }
            }

            texture.Apply();
            return texture;
        }



        private void SaveTexture(Texture2D texture)
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Save Block Texture",
                "BlockTexture.png",
                "png",
                "Please enter a file name to save the texture to"
            );

            if (!string.IsNullOrEmpty(path))
            {
                byte[] pngData = texture.EncodeToPNG();
                FileUtils.WriteAllBytes(path, pngData);
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Saved", "Texture saved to Assets!", "OK");
            }
        }

        private Texture2D Upscale(Texture2D src, int factor)
        {
            int newSize = src.width * factor;
            Texture2D scaled = new Texture2D(newSize, newSize);
            scaled.filterMode = FilterMode.Point;

            for (int y = 0; y < newSize; y++)
            {
                for (int x = 0; x < newSize; x++)
                {
                    Color c = src.GetPixel(x / factor, y / factor);
                    scaled.SetPixel(x, y, c);
                }
            }

            scaled.Apply();
            return scaled;
        }
    }
}
