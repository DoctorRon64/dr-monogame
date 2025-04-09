using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;

namespace ProDevs.Framework.ECS.System {
    public class RenderSystem {
        private readonly List<Entity.Entity> renderableEntities = new();
        public RenderSystem() => Console.WriteLine("Initializing RenderSystem");

        public void Register(Entity.Entity entity) => renderableEntities.Add(entity);
        public void Unregister(Entity.Entity entity) => renderableEntities.Remove(entity);
        
        public void Draw(SpriteBatch spriteBatch) {
            
            foreach (Entity.Entity entity in renderableEntities) {
                SpriteComponent sprite = entity.GetComponent<SpriteComponent>();
                TransformComponent transform = entity.GetComponent<TransformComponent>();

                if (sprite.GetTexture() == null) continue;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.Draw(sprite.GetTexture(), transform.Position, null, sprite.GetColor(),
                    transform.Rotation, transform.Origin, transform.Scale, sprite.GetSpriteEffects(), 0);
                spriteBatch.End();
            }
        }
    }
}