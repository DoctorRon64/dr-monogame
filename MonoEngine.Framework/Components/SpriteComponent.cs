using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace MonoEngine.Framework;

public class SpriteComponent : Component {
    private Texture2D texture = null;
    private Vector2 spriteOffset = Vector2.Zero;
    private Color color = Color.White;
    private SpriteEffects effects = SpriteEffects.None;

    private string texturePath = string.Empty;
    
    private int TextureWidth => texture?.Width ?? 0;
    private int TextureHeight => texture?.Height ?? 0;

    public void SetTexture(string assetName, ContentManager content) {
        Texture2D result = content.Load<Texture2D>(assetName);
        if (result == null) {
            Console.WriteLine("ERROR: Texture not found: " + assetName);
            return;
        }

        texture = result;
        texturePath = assetName;
    }
        
    public string GetTexturePath() => texturePath;
    public string SetTexturePath(string assetName) => texturePath = assetName;
    
    public Vector2 GetSize() => new(TextureWidth, TextureHeight);
    public System.Numerics.Vector2 GetSizeN() => new(TextureWidth, TextureHeight);
    public Texture2D GetTexture() => texture;
        
    public Color GetColor() => color;
    public void SetColor(Color newColor) => color = newColor;
    public SpriteEffects GetSpriteEffects() => effects;
    public void SetSpriteEffects(SpriteEffects newEffects) => newEffects = newEffects;
        
    public void SetOffset(Vector2 offset) => spriteOffset = offset;
    public Vector2 GetOffset() => spriteOffset;
    public System.Numerics.Vector2 GetOffsetN() => new(spriteOffset.X, spriteOffset.Y);
}