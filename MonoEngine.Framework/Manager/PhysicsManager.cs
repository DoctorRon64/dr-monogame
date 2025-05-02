using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Framework.components;
using MonoEngine.Framework.Components;

namespace MonoEngine.Framework.Manager
{
    public class PhysicsSystem : BaseSingleton<PhysicsSystem>
    {
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in SceneManager.Instance.Entities)
            {
                Velocity? vel = entity.GetComponent<Velocity>();
                PhysicsBody? rb = entity.GetComponent<PhysicsBody>();

                vel.Value += rb.Acceleration * dt;
                rb.Acceleration = Vector2.Zero;

            }
        }
    }
}