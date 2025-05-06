using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Framework.components;

namespace MonoEngine.Framework.Manager {
    public class PhysicsSystem : BaseSingleton<PhysicsSystem> {
        public void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Entity entity in SceneManager.Instance.Entities) {
                entity.TryGetComponent(out PhysicsBody physicsBody);
                entity.TryGetComponent(out RigidBody rb);

                if (physicsBody == null || rb == null) continue;
                
                physicsBody.Velocity += rb.Acceleration * dt;
                rb.Acceleration = Vector2.Zero;
            }
        }
    }
}