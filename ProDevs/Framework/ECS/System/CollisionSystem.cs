using System.Collections.Generic;
using System.Linq;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;
using ProDevs.Framework.Interfaces;

namespace ProDevs.Framework.ECS.System {
    public class CollisionSystem {
        private SpatialGrid spatialGrid = new SpatialGrid(500);
        private Dictionary<GameObject, List<GameObject>> previousCollisions = new();

        public void Update(List<GameObject> gameObjects) {
            spatialGrid.Clear();

            foreach (GameObject obj in gameObjects) {
                if (!obj.TryGetComponent(out Collider collider) ||
                    !obj.TryGetComponent(out TransformComponent transform)) continue;
                collider.UpdateBounds(transform);
                spatialGrid.AddCollider(collider);
            }

            foreach (GameObject obj in gameObjects) {
                if (!obj.TryGetComponent(out Collider collider))
                    continue;

                List<GameObject> newCollisions = new();
                List<Collider> nearbyColliders = spatialGrid.GetNearbyColliders(collider);

                foreach (GameObject otherObj in from otherCollider in nearbyColliders
                         where otherCollider != collider
                         let otherObj = otherCollider.GetEntity()
                         where collider.Bounds.Intersects(otherCollider.Bounds)
                         select otherObj) {
                    newCollisions.Add(otherObj);

                    if (obj.TryGetComponent(out RigidBodyComponent rb)) {
                        rb.HandleCollision(otherObj); // Stop movement upon collision
                    }

                    if (!previousCollisions.TryGetValue(obj, out List<GameObject> prev) || !prev.Contains(otherObj)) {
                        collider.OnCollisionEnter?.Invoke(otherObj);
                    }
                    else {
                        collider.OnCollisionStay?.Invoke(otherObj);
                    }
                }

                if (previousCollisions.TryGetValue(obj, out List<GameObject> prevCollisions)) {
                    foreach (GameObject prevObj in prevCollisions.Where(prevObj => !newCollisions.Contains(prevObj))) {
                        collider.OnCollisionExit?.Invoke(prevObj);
                    }
                }

                previousCollisions[obj] = newCollisions;
            }
        }
    }
}