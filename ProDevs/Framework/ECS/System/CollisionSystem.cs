using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using ProDevs.Framework.ECS.Components;

namespace ProDevs.Framework.ECS.System {
    public class CollisionSystem {
        private BvhNode root;

        public void BuildBvh(List<Collider> colliders) => root = BvhNode.Build(colliders);

        public List<(Collider, Collider)> CheckCollisions() {
            List<(Collider, Collider)> collisions = new();
            CheckCollisionsRecursive(root, collisions);
            return collisions;
        }
        
        public void CheckCollisions(out List<(Collider, Collider)> collisions) {
            collisions = new();
            CheckCollisionsRecursive(root, collisions);
        }

        private static void CheckCollisionsRecursive(BvhNode node, List<(Collider, Collider)> collisions) {
            while (true) {
                if (node == null) return;
                if (node.Colliders.Count > 1) {
                    for (int i = 0; i < node.Colliders.Count; i++) {
                        for (int j = i + 1; j < node.Colliders.Count; j++) {
                            if (!node.Colliders[i].Intersects(node.Colliders[j])) continue;
                            collisions.Add((node.Colliders[i], node.Colliders[j]));
                            Console.WriteLine($"Collision: {node.Colliders[i].Owner.Id} vs {node.Colliders[j].Owner.Id}");
                        }
                    }
                }
                if (node.Left != null && node.Right != null && node.Left.Bounds.Intersects(node.Right.Bounds)) {
                    foreach (Collider leftCollider in node.Left.GetAllColliders()) {
                        foreach (Collider rightCollider in node.Right.GetAllColliders()
                                     .Where(rightCollider => leftCollider.Intersects(rightCollider))) {
                            collisions.Add((leftCollider, rightCollider));
                            Console.WriteLine($"Collision: {leftCollider.Owner.Id} vs {rightCollider.Owner.Id}");
                        }
                    }
                }

                CheckCollisionsRecursive(node.Left, collisions);
                node = node.Right;
            }
        }
    }

    public class BvhNode {
        public BvhNode Left, Right;
        public List<Collider> Colliders;
        public Rectangle Bounds;

        private BvhNode(List<Collider> colliders) {
            Colliders = colliders;
            Bounds = colliders.Select(c => c.Bounds).Aggregate(Rectangle.Union);
        }

        private BvhNode(BvhNode left, BvhNode right) {
            Left = left;
            Right = right;
            Colliders = new();
            Bounds = Rectangle.Union(left.Bounds, right.Bounds);
        }

        public List<Collider> GetAllColliders() {
            List<Collider> colliders = new(Colliders);
            if (Left != null) colliders.AddRange(Left.GetAllColliders());
            if (Right != null) colliders.AddRange(Right.GetAllColliders());
            return colliders;
        }

        public static BvhNode Build(List<Collider> colliders) {
            if (colliders.Count == 1) return new BvhNode(colliders);

            colliders.Sort((a, b) => a.Bounds.X.CompareTo(b.Bounds.X));
            int mid = colliders.Count / 2;

            return new(
                Build(colliders.GetRange(0, mid)),
                Build(colliders.GetRange(mid, colliders.Count - mid))
            );
        }
    }
}