using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;

namespace ProDevs.Framework.ECS.System {
    public class RenderSystem {
        private readonly List<GameObject> entities = new();
  
        public RenderSystem() {
            Console.WriteLine("Initializing RenderSystem");
        }

        public void Register(GameObject gameObject) => entities.Add(gameObject);
        public void Unregister(GameObject gameObject) => entities.Remove(gameObject);
        
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            foreach (GameObject entity in entities) {
                if (!entity.TryGetComponent(out TransformComponent transform) ||
                    !entity.TryGetComponent(out SpriteComponent sprite) || sprite.Texture == null) continue;
                spriteBatch.Draw(sprite.Texture, transform.Position, null, sprite.Color,
                    transform.Rotation, transform.Origin, transform.Scale, sprite.Effects, 0);
            }
            
            spriteBatch.End();
        }
    }
}