using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGuiNet;

namespace MonoEngine.Utility;

public static class AssetRegistry
{
    public static List<string> SpritePaths { get; private set; } = [];
    private static Dictionary<string, IntPtr> textureHandles = new();

    public static IntPtr GetThumbnail(string assetPath, ContentManager content, ImGuiRenderer imguiRenderer)
    {
        if (textureHandles.TryGetValue(assetPath, out IntPtr handle))
            return handle;

        Texture2D texture = content.Load<Texture2D>(assetPath);
        IntPtr imguiHandle = imguiRenderer.BindTexture(texture); // <- Important!
        textureHandles[assetPath] = imguiHandle;

        return imguiHandle;
    }

    public static void Refresh()
    {
        string baseDir = AppContext.BaseDirectory;
        string contentPath = Path.GetFullPath(Path.Combine(baseDir, "../../../Assets/Content.mgcb"));

        if (!File.Exists(contentPath))
        {
            Console.WriteLine("[AssetRegistry] Content.mgcb not found at: " + contentPath);
            return;
        }

        List<string> assetNames = new();
        foreach (string line in File.ReadLines(contentPath))
        {
            if (!line.StartsWith("/build:")) continue;
            string fullPath = line.Split(":")[1].Trim();

            if (!fullPath.Contains(';')) continue;
            string mappedName = fullPath.Split(';')[1];
            string assetPath = Path.ChangeExtension(mappedName, null).Replace("\\", "/"); // Remove extension
            assetNames.Add(assetPath);
            Console.WriteLine($"[AssetRegistry] Asset from mgcb: {assetPath}");
        }

        SpritePaths = assetNames;
    }
}