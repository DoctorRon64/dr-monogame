using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGuiNet;

namespace MonoEngine.Utility;

public static class AssetRegistry {
    public static List<string> SpritePaths { get; private set; } = [];
    private static readonly Dictionary<string, IntPtr> textureHandles = new();

    public static void RefreshPaths() {
        // Only refresh the list of asset paths, no loading yet
        string baseDir = AppContext.BaseDirectory;
        string contentPath = Path.GetFullPath(Path.Combine(baseDir, "../../../Content/Content.mgcb"));

        if (!File.Exists(contentPath)) {
            Console.WriteLine("[AssetRegistry] Content.mgcb not found at: " + contentPath);
            return;
        }

        List<string> assetNames = new();
        foreach (string line in File.ReadLines(contentPath)) {
            if (!line.StartsWith("/build:")) continue;
            string path = line[7..].Trim();
            string assetName = path.Contains(';')
                ? Path.ChangeExtension(path.Split(';')[1].Trim(), null).Replace("\\", "/")
                : Path.ChangeExtension(path, null).Replace("\\", "/");

            assetNames.Add(assetName);
            Console.WriteLine($"[AssetRegistry] Found asset: {assetName}");
        }

        SpritePaths = assetNames;
    }

    public static void ReloadContent(ContentManager content, ImGuiRenderer imguiRenderer) {
        Console.WriteLine("[AssetRegistry] Reloading content...");

        textureHandles.Clear(); // Clear cached textures

        foreach (string assetPath in SpritePaths) {
            try {
                Texture2D texture = content.Load<Texture2D>(assetPath);
                IntPtr imguiHandle = imguiRenderer.BindTexture(texture);
                textureHandles[assetPath] = imguiHandle;

                Console.WriteLine($"[AssetRegistry] Loaded {assetPath}");
            }
            catch (Exception ex) {
                Console.WriteLine($"[AssetRegistry] Failed to load {assetPath}: {ex.Message}");
            }
        }
    }

    private static IntPtr missingTextureHandle = IntPtr.Zero;
    private static bool missingTextureLoaded = false;

    public static IntPtr GetThumbnail(string assetPath, GameManager gameManager) {
        if (textureHandles.TryGetValue(assetPath, out var handle))
            return handle;

        if (missingTextureLoaded) return missingTextureHandle;

        Texture2D missingTex = CreateMissingTexture(gameManager);
        missingTextureHandle = gameManager.ImGuiRenderer.BindTexture(missingTex);
        missingTextureLoaded = true;

        return missingTextureHandle;
    }

    private static Texture2D CreateMissingTexture(GameManager gameManager) {
        Texture2D tex = new Texture2D(gameManager.GraphicsDevice, 2, 2);
        tex.SetData(new[] {
            Color.Magenta, Color.Black,
            Color.Black, Color.Magenta
        });
        return tex;
    }
}