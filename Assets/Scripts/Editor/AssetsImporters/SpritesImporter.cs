using UnityEngine;
using UnityEditor;

public class SpritesImporter : AssetPostprocessor
{
    const string terrainSpritesPath = "Assets/Textures/Board/Terrain/";

    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        if (textureImporter.assetPath.StartsWith(terrainSpritesPath))
        {
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.filterMode = FilterMode.Point;

            TextureImporterPlatformSettings textureSettings = textureImporter.GetDefaultPlatformTextureSettings();
            textureSettings.format = TextureImporterFormat.RGBA32;
            textureSettings.textureCompression = TextureImporterCompression.Uncompressed;
            textureImporter.SetPlatformTextureSettings(textureSettings);
        }
    }
}