using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoEngine.Utility;

public static class AssetRegistry {
    public static List<string> SpritePaths { get; private set; } = [];

    public static void Refresh() {
        string contentDir = Path.Combine(AppContext.BaseDirectory, "Content", "sprites");

        if (!Directory.Exists(contentDir)) {
            Console.WriteLine($"[AssetRegistry] Folder not found: {contentDir}");
            return;
        }

        SpritePaths = Directory
            .EnumerateFiles(contentDir, "*.xnb", SearchOption.AllDirectories)
            .Select(path => {
                string relative = Path.GetRelativePath(contentDir, path);
                string assetPath = Path.ChangeExtension(relative, null).Replace("\\", "/");
                Console.WriteLine($"[AssetRegistry] Found asset: {assetPath}");
                return assetPath;
            })
            .ToList();
    }
}