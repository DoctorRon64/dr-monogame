namespace MonoEngine.Framework.Utility;

public static class AssetRegistry {
    public static List<string> SpritePaths { get; private set; } = [];

    public static void Refresh() {
        string spriteDir = Path.Combine(AppContext.BaseDirectory, "Content", "Sprites");
        if (!Directory.Exists(spriteDir)) return;

        SpritePaths = new List<string> { "Content/mexican.jpg" }; // Add a hardcoded path for testing

        /*SpritePaths = Directory
            .EnumerateFiles(spriteDir, "*.*", SearchOption.AllDirectories)
            .Where(file => file.EndsWith(".png") || file.EndsWith(".jpg"))
            .Select(path => {
                string relative = Path.GetRelativePath(Path.Combine(AppContext.BaseDirectory, "Assets"), path);
                string assetPath = Path.ChangeExtension(relative, null).Replace("\\", "/");
                Console.WriteLine($"Asset found: {assetPath}"); // Log the asset path
                return assetPath;
            })
            .ToList();*/
    }
}
