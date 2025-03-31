using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ProDevs.Framework.ECS.Components;

namespace ProDevs.Framework.ECS.System {
    public class CollisionSystem {
        private BvhNode root;

        public void BuildBvh(List<Collider> colliders) {
            root = BuildBvhRecursive(colliders);
        }

        private static BvhNode BuildBvhRecursive(List<Collider> colliders) {
            if (colliders.Count == 1) return new BvhNode(colliders[0]);

            colliders.Sort((a, b) => a.Bounds.X.CompareTo(b.Bounds.X)); // Sort by X axis
            int mid = colliders.Count / 2;
            return new BvhNode(
                BuildBvhRecursive(colliders.GetRange(0, mid)),
                BuildBvhRecursive(colliders.GetRange(mid, colliders.Count - mid))
            );
        }

        public List<(Collider, Collider)> CheckCollisions() {
            List<(Collider, Collider)> collisions = new();
            CheckCollisionsRecursive(root, collisions);
            return collisions;
        }

        private static void CheckCollisionsRecursive(BvhNode node, List<(Collider, Collider)> collisions) {
            if (node is not { Collider: null }) return;

            if (node.Left != null && node.Right != null && node.Left.Bounds.Intersects(node.Right.Bounds)) {
                collisions.AddRange(from left in node.Left.GetAllColliders()
                    from right in node.Right.GetAllColliders()
                    where left.Intersects(right)
                    select (left, right));
            }

            CheckCollisionsRecursive(node.Left, collisions);
            CheckCollisionsRecursive(node.Right, collisions);
        }
    }

    public class BvhNode {
        public BvhNode Left, Right;
        public Collider Collider;
        public Rectangle Bounds;

        public BvhNode(Collider collider) {
            Collider = collider;
            Bounds = collider.Bounds;
        }

        public BvhNode(BvhNode left, BvhNode right) {
            Left = left;
            Right = right;
            Bounds = Rectangle.Union(left.Bounds, right.Bounds);
        }

        public List<Collider> GetAllColliders() {
            List<Collider> colliders = new();
            if (Collider != null) colliders.Add(Collider);
            if (Left != null) colliders.AddRange(Left.GetAllColliders());
            if (Right != null) colliders.AddRange(Right.GetAllColliders());
            return colliders;
        }
    }
}