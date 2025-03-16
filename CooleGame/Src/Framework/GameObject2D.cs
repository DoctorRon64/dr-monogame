using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CooleGame.Framework
{
    public class GameObject2D {
        public Transform2D Transform2D = new();
        private Sprite2D sprite = new();
        public Vector2 Velocity { get; private set; } = Vector2.Zero;
        public Vector2 Acceleration { get; private set; } = Vector2.Zero;
        public float DragFactor { get; private set; } = 5f;
        
        public void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Velocity += Acceleration * deltaTime;
            Velocity *= 1 - (DragFactor * deltaTime);
            Transform2D.Position += Velocity * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (sprite.Texture == null) return;
            spriteBatch.Draw(sprite.Texture, Transform2D.Position, null, sprite.Color, Transform2D.Rotation, Transform2D.Origin, Transform2D.Scale, SpriteEffects.None, 0);
        }
    
        public void SetTexture(Texture2D texture) => sprite.Texture = texture;
        public void SetPosition(Vector2 position) => Transform2D.Position = position;
        public void SetRotation(float rotation) => Transform2D.Rotation = rotation;
        public void SetScale(Vector2 scale) => Transform2D.Scale = scale;
        public void SetVelocity(float x, float y) => Velocity = new Vector2(x, y);
        public void Move(float x, float y, float speed = 1f) => Velocity += new Vector2(x * speed, y * speed);
    }
}