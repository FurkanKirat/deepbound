using System.Collections.Generic;
using System.IO;
using System.Linq;
using Constants;
using Constants.Paths;
using Data.Database;
using Data.Serializable;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editor.Automation
{
    public static class AtlasBuilderEditor
    {
        public static void Run()
        {
            const string sourceFolder = AtlasPaths.AtlasSourceFolder;


            var files = Directory.GetFiles(sourceFolder, "*.png")
                .Where(f => !f.Contains("meta"))
                .OrderBy(f => f)
                .ToList();
            
            if (files.Count == 0)
            {
                GameLogger.Warn("No tile images found.", nameof(AtlasBuilderEditor));
                return;
            }
            
            var grouped = files.GroupBy(f =>
            {
                var name = Path.GetFileNameWithoutExtension(f);
                var parts = name.Split('_');
                return parts[0]; // block ID
            });

            // Create atlas
            Texture2D atlas = new Texture2D(TileAtlasConstants.AtlasPixelCount, TileAtlasConstants.AtlasPixelCount, TextureFormat.RGBA32, false);
            Dictionary<string, BlockAtlasSaveData> uvRects = new();
            int count = 0;

            foreach (var group in grouped)
            {
                string blockId = $"block:{group.Key}";
                var frames = new List<AtlasSaveData>();

                foreach (var file in group.OrderBy(f => f))
                {
                    byte[] bytes = File.ReadAllBytes(file);
                    Texture2D tile = new Texture2D(2, 2);
                    tile.LoadImage(bytes);

                    const int tilesPerRow = TileAtlasConstants.GridCountPerRow;
                    const int tileSize = TileAtlasConstants.GridPixelCount;

                    var size = new Int2(tile.width / tileSize, tile.height / tileSize);
                    var positions = new Int2[size.x * size.y];
                    int localCount = 0;

                    for (int b = 0; b < size.y; b++)
                    for (int a = 0; a < size.x; a++)
                    {
                        var gridPos = RectUtils.GetCoordinate(count, tilesPerRow);
                        positions[localCount] = gridPos;
                        var destX = gridPos.x * tileSize;
                        var destY = gridPos.y * tileSize;

                        var srcX = a * tileSize;
                        var srcY = b * tileSize;

                        for (int y = 0; y < tileSize; y++)
                        for (int x = 0; x < tileSize; x++)
                        {
                            var color = tile.GetPixel(srcX + x, srcY + y);
                            atlas.SetPixel(destX + x, destY + y, color);
                        }

                        count++;
                        localCount++;
                    }

                    frames.Add(new AtlasSaveData(positions, size));
                }

                uvRects.Add(blockId, new BlockAtlasSaveData(frames.ToArray()));
            }

            if (count > TileAtlasConstants.MaxTileCount)
            {
                GameLogger.Warn($"Too many tiles! Max allowed in 512x512 atlas is {TileAtlasConstants.MaxTileCount}, but found {count}.", nameof(AtlasBuilderEditor));
                return;
            }

            atlas.Apply();
            const string outputPath = AtlasPaths.AtlasOutputPath;
            FileUtils.WriteAllBytes(outputPath, atlas.EncodeToPNG());

            if (!outputPath.StartsWith("Assets"))
            {
                GameLogger.Error($"❌ outputPath is not relative to Assets: {outputPath}", nameof(AtlasBuilderEditor));
                return;
            }

            TextureImporter importer = AssetImporter.GetAtPath(outputPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.filterMode = FilterMode.Point;
                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.spriteImportMode = SpriteImportMode.Single;
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();

                AtlasUVIndex.SaveUVData(uvRects);
                GameLogger.Log($"✅ Tile Atlas created at 512x512 with {files.Count} tiles and animations.", nameof(AtlasBuilderEditor));
            }
            else
            {
                GameLogger.Error($"❌ Could not find TextureImporter for path: {outputPath}", nameof(AtlasBuilderEditor));
            }
        }
    }
}
