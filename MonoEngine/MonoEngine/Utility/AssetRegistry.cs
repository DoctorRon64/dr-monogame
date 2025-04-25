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

        var assetNames = new List<string>();

        foreach (string line in File.ReadLines(contentPath))
        {
            if (line.StartsWith("/build:"))
            {
                string path = line[7..].Trim(); // Skip "/build:"
                if (path.Contains(';'))
                {
                    string alias = path.Split(';')[1].Trim();
                    string assetName = Path.ChangeExtension(alias, null).Replace("\\", "/");
                    assetNames.Add(assetName);
                    Console.WriteLine($"[AssetRegistry] Aliased asset: {assetName}");
                }
                else
                {
                    string assetName = Path.ChangeExtension(path, null).Replace("\\", "/");
                    assetNames.Add(assetName);
                    Console.WriteLine($"[AssetRegistry] Direct asset: {assetName}");
                }
            }
        }

        SpritePaths = assetNames;
    }
}