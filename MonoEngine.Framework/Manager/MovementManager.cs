using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Framework.components;
using MonoEngine.Framework.Components;

namespace MonoEngine.Framework.Manager
{
    public class MovementManager : BaseSingleton<MovementManager>
    {
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in SceneManager.Instance.Entities)
            {
                Transform? transform = entity.GetComponent<Transform>();
                Velocity velocity = entity.GetComponent<Velocity>();

                transform.Position += velocity.Value * dt;
            }
        }
    }
}