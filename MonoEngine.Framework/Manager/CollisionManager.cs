using Microsoft.Xna.Framework;

namespace MonoEngine.Framework;

public class CollisionManager(BvhNode root) {
    private BvhNode root = root;

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
        if (node == null) return;

        for (int i = 0; i < node.Colliders.Count; i++) {
            for (int j = i + 1; j < node.Colliders.Count; j++) {
                Collider a = node.Colliders[i];
                Collider b = node.Colliders[j];

                if (!a.Intersects(b)) continue;

                collisions.Add((a, b));
                Console.WriteLine($"Collision: {a.Owner.Id} vs {b.Owner.Id}");

                if (a.Owner.Id < b.Owner.Id)
                    a.ResolveCollision(b);
            }
        }

        if (node.Left != null && node.Right != null && node.Left.Bounds.Intersects(node.Right.Bounds)) {
            List<Collider> left = node.Left.GetAllColliders();
            List<Collider> right = node.Right.GetAllColliders();

            foreach (Collider a in left) {
                foreach (Collider b in right.Where(b => a.Intersects(b))) {
                    if (!a.Intersects(b)) continue;

                    collisions.Add((a, b));
                    Console.WriteLine($"Collision: {a.Owner.Id} vs {b.Owner.Id}");

                    if (a.Owner.Id < b.Owner.Id)
                        a.ResolveCollision(b);
                }
            }
        }

        CheckCollisionsRecursive(node.Left, collisions);
        CheckCollisionsRecursive(node.Right, collisions);
    }

    public static void ApplyResolution(Collider a, Collider b, Vector2 push) {
        Transform tA = a.Transform;
        Transform tB = b.Transform;

        RigidBodyComponent rbA = a.Owner.GetComponent<RigidBodyComponent>();
        RigidBodyComponent rbB = b.Owner.GetComponent<RigidBodyComponent>();

        bool aKinematic = rbA?.IsKinematic ?? true;
        bool bKinematic = rbB?.IsKinematic ?? true;

        switch (aKinematic) {
            case false when bKinematic && tA != null:
                tA.Position += push;
                rbA?.HandleCollision(b.Owner);
                break;
            case true when !bKinematic && tB != null:
                tB.Position -= push;
                rbB?.HandleCollision(a.Owner);
                break;
            case false when !bKinematic && tA != null && tB != null:
                tA.Position += push / 2f;
                tB.Position -= push / 2f;
                rbA?.HandleCollision(b.Owner);
                rbB?.HandleCollision(a.Owner);
                break;
        }

        a.UpdateBounds();
        b.UpdateBounds();
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