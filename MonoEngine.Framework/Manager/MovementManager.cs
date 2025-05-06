using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Framework.components;

namespace MonoEngine.Framework.Manager {
    public class MovementManager : BaseSingleton<MovementManager> {
        public void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Entity entity in SceneManager.Instance.Entities) {
                entity.TryGetComponent(out Transform transform);
                entity.TryGetComponent(out PhysicsBody physicsBody);

                if (transform == null || physicsBody == null) continue;
                
                transform.Position += physicsBody.Velocity * dt;
            }
        }
    }
}