using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.Interfaces;

namespace ProDevs.Framework.Components {
    public class SpriteComponent() : Component, IRenderable {
        public Texture2D Texture = null;
        public Vector2 SpriteOffset = Vector2.Zero;
        public Color Color = Color.White;

        public override void Update(float deltaTime) { }

        public void Draw(SpriteBatch spriteBatch) {
            if (Texture == null) return;
            spriteBatch.Draw(Texture, SpriteOffset, Color);
        }

        public void SetTexture(Texture2D texture) => Texture = texture;
        public void Offset(Vector2 offset) => SpriteOffset = offset;
    }
}