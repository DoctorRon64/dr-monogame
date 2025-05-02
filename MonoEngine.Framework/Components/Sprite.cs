using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoEngine.Framework.components;

public class Sprite : Component
{
    public Texture2D Texture { get; private set; } = null!;
    public Vector2 Offset = Vector2.Zero;
    public Color Color = Color.White;
    public SpriteEffects Effects = SpriteEffects.None;

    private string texturePath = string.Empty;
    private int TextureWidth => Texture?.Width ?? 0;
    private int TextureHeight => Texture?.Height ?? 0;

    public void SetTexture(string texturePath, ContentManager content)
    {
        Texture2D result = content.Load<Texture2D>(texturePath);
        if (result == null)
        {
            Console.WriteLine("ERROR: Texture not found: " + texturePath);
            return;
        }

        Texture = result;
        this.texturePath = texturePath;
    }

    public void SetTexture(Texture2D texture) => Texture = texture;
    public string GetTexturePath() => texturePath;
    public string SetTexturePath(string assetName) => texturePath = assetName;

    public Vector2 GetSize() => new(TextureWidth, TextureHeight);
    public System.Numerics.Vector2 GetSizeN() => new(TextureWidth, TextureHeight);
    public System.Numerics.Vector2 GetOffsetN() => new(Offset.X, Offset.Y);
}