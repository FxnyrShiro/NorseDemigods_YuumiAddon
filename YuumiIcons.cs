using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace NorseDemigods.Yuumi;

/// <summary>
///     Loads the embedded League of Legends icons (Data Dragon assets) and turns them
///     into Unity textures/sprites.
/// </summary>
internal static class YuumiIcons
{
    private const string ResourcePrefix = "NorseDemigodsYuumi.Icons.";

    private static Sprite _championSprite;

    internal static Texture2D Champion { get; private set; }

    internal static Texture2D Passive { get; private set; }

    internal static Texture2D Q { get; private set; }

    internal static Texture2D W { get; private set; }

    internal static Texture2D E { get; private set; }

    internal static Texture2D R { get; private set; }

    internal static Sprite ChampionSprite =>
        _championSprite != null ? _championSprite : (_championSprite = ToSprite(Champion));

    internal static void Load()
    {
        Champion = LoadTexture("Yuumi.png");
        Passive = LoadTexture("YuumiP2.png");
        Q = LoadTexture("YuumiQ.png");
        W = LoadTexture("YuumiW.png");
        E = LoadTexture("YuumiE.png");
        R = LoadTexture("YuumiR.png");
    }

    internal static Sprite ToSprite(Texture2D texture)
    {
        if (texture == null)
        {
            return null;
        }

        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private static Texture2D LoadTexture(string fileName)
    {
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(ResourcePrefix + fileName))
            {
                if (stream == null)
                {
                    YuumiPlugin.Log.LogWarning($"Embedded icon not found: {ResourcePrefix + fileName}");
                    return null;
                }

                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.name = "Yuumi_" + Path.GetFileNameWithoutExtension(fileName);
                texture.filterMode = FilterMode.Bilinear;
                texture.wrapMode = TextureWrapMode.Clamp;
                if (!ImageConversion.LoadImage(texture, buffer))
                {
                    YuumiPlugin.Log.LogWarning($"Could not decode embedded icon: {fileName}");
                    return null;
                }

                return texture;
            }
        }
        catch (Exception exception)
        {
            YuumiPlugin.Log.LogError($"Failed to load icon '{fileName}': {exception}");
            return null;
        }
    }
}
