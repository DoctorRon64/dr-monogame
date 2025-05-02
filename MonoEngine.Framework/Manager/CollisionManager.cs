using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Framework.components;
using MonoEngine.Framework.Components;

namespace MonoEngine.Framework.Manager
{
    public class CollisionSystem : BaseSingleton<CollisionSystem>
    {
        public void Update(GameTime gameTime)
        {
            foreach (var entity in SceneManager.Instance.Entities)
            {
                Transform? transform = entity.GetComponent<Transform>();
                Collider collider = entity.GetComponent<Collider>();

                if (collider.IsStatic) continue;

                Rectangle bounds = new(
                   (int)(transform.Position.X + collider.Bounds.X),
                   (int)(transform.Position.Y + collider.Bounds.Y),
                   (int)collider.Bounds.Width,
                   (int)collider.Bounds.Height
               );

                foreach (var other in SpatialPartitionManager.Query(bounds))
                {
                    if (other == entity) continue;

                    var othertransform = other.GetComponent<Transform>();
                    var otherCol = other.GetComponent<Collider>();

                    Rectangle otherBounds = new(
                        (int)(othertransform.Position.X + otherCol.Bounds.X),
                        (int)(othertransform.Position.Y + otherCol.Bounds.Y),
                        (int)otherCol.Bounds.Width,
                        (int)otherCol.Bounds.Height
                    );

                    if (bounds.Intersects(otherBounds))
                    {
                        Vector2 mtv = GetMTV(bounds, otherBounds);
                        transform.Position -= mtv;

                        var vel = entity.GetComponent<Velocity>();
                        if (vel != null) vel.Value = Vector2.Zero;
                    }
                }

            }
        }

        private Vector2 GetMTV(Rectangle a, Rectangle b)
        {
            float dx = (a.Center.X - b.Center.X);
            float px = (a.Width / 2 + b.Width / 2) - Math.Abs(dx);

            float dy = (a.Center.Y - b.Center.Y);
            float py = (a.Height / 2 + b.Height / 2) - Math.Abs(dy);

            if (px < py)
                return new Vector2(dx < 0 ? -px : px, 0);
            else
                return new Vector2(0, dy < 0 ? -py : py);
        }
    }
}