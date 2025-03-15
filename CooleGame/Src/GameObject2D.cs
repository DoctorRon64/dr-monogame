using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CooleGame;

public class GameObject2D {
    public Transform2D Transform2D = new();
    private Sprite2D Sprite = new();
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;
    
    public void Update(SpriteBatch spriteBatch) {
        Transform2D.Position += Velocity;
        Velocity.Y *= 9.81f;
        
        spriteBatch.Begin();
        spriteBatch.Draw(Sprite.Texture, Transform2D.Position, null, Sprite.Color, Transform2D.Rotation, Transform2D.Origin, Transform2D.Scale, SpriteEffects.None, 0);
        spriteBatch.End();
    }

    public void SetTexture(Texture2D texture) {
        Sprite.Texture = texture;
    }

    public void SetPosition(Vector2 position) {
        Transform2D.Position = position;
    }

    public void SetScale(Vector2 scale) {
        Transform2D.Scale = scale;
    }
    
    public void SetVelocity(float x, float y) {
        Velocity.X = x;
        Velocity.Y = y;
    }
    
    public void Move(float x, float y) {
        Velocity.X += x;
        Velocity.Y += y;
    }
}