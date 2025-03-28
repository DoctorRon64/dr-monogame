using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;

namespace ProDevs.Framework.ECS.System {
    public class RenderSystem {
        private readonly List<GameObject> renderableEntities = new();
  
        public RenderSystem() {
            Console.WriteLine("Initializing RenderSystem");
        }

        public void Register(GameObject gameObject) => renderableEntities.Add(gameObject);
        public void Unregister(GameObject gameObject) => renderableEntities.Remove(gameObject);
        
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            foreach (GameObject entity in renderableEntities) {
                if (!entity.TryGetComponent(out TransformComponent transform) ||
                    !entity.TryGetComponent(out SpriteComponent sprite) || sprite.GetTexture() == null) continue;
                spriteBatch.Draw(sprite.GetTexture(), transform.Position, null, sprite.GetColor(),
                    transform.Rotation, transform.Origin, transform.Scale, sprite.GetSpriteEffects(), 0);
            }
            
            spriteBatch.End();
        }
    }
}