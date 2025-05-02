using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoEngine.Framework.components;
using MonoEngine.Framework.Components;

namespace MonoEngine.Framework.Manager {
    public class CollisionSystem : BaseSingleton<CollisionSystem> {
        public void Update(GameTime gameTime) {
            foreach (Entity entity in SceneManager.Instance.Entities) {
                Transform? transform = entity.GetComponent<Transform>();
                Collider collider = entity.GetComponent<Collider>();

                if (collider.IsStatic) continue;

                Rectangle bounds = new(
                    (int)(transform.Position.X + collider.Bounds.X),
                    (int)(transform.Position.Y + collider.Bounds.Y),
                    (int)collider.Bounds.Width,
                    (int)collider.Bounds.Height
                );

                foreach (Entity other in SpatialPartitionManager.Query(bounds)) {
                    if (other == entity) continue;

                    Transform othertransform = other.GetComponent<Transform>();
                    Collider otherCol = other.GetComponent<Collider>();

                    Rectangle otherBounds = new(
                        (int)(othertransform.Position.X + otherCol.Bounds.X),
                        (int)(othertransform.Position.Y + otherCol.Bounds.Y),
                        (int)otherCol.Bounds.Width,
                        (int)otherCol.Bounds.Height
                    );

                    if (!bounds.Intersects(otherBounds)) continue;
                    Vector2 mtv = GetMinimumTranslationVector(bounds, otherBounds);
                    transform.Position -= mtv;

                    Velocity? vel = entity.GetComponent<Velocity>();
                    if (true) vel.Value = Vector2.Zero;
                }
            }
        }

        private static Vector2 GetMinimumTranslationVector(Rectangle a, Rectangle b) {
            float deltaX = a.Center.X - b.Center.X;
            float overlapX = (a.Width / 2f + b.Width / 2f) - Math.Abs(deltaX);

            float deltaY = a.Center.Y - b.Center.Y;
            float overlapY = (a.Height / 2f + b.Height / 2f) - Math.Abs(deltaY);

            if (overlapX <= 0 || overlapY <= 0) return Vector2.Zero;
            return overlapX < overlapY
                ? new(deltaX < 0 ? -overlapX : overlapX, 0)
                : new(0, deltaY < 0 ? -overlapY : overlapY);
        }
    }
}