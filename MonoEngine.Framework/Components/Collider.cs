using Microsoft.Xna.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoEngine.Framework {
    public abstract class Collider(Entity owner) : Component {
        public Rectangle Bounds { get; protected set; }
        public bool IsTrigger = false;
        public Entity Owner { get; private set; } = owner;
        public Transform Transform => Owner?.GetComponent<Transform>();
        public abstract void UpdateBounds();
        public abstract bool Intersects(Collider other);
        public abstract void ResolveCollision(Collider other);
        public abstract void ResolveAgainst(BoxCollider box);
        public abstract void ResolveAgainst(CircleCollider circle);
    }
    
    public sealed class BoxCollider : Collider {
        public Vector2 Size { get; private set; }
    
        public BoxCollider(Entity owner, Vector2 size) : base(owner) {
            Size = size;
            UpdateBounds();
        }

        public override void UpdateBounds() {
            if (!Owner.TryGetComponent(out Transform transform)) return;
            Bounds = new Rectangle(
                (int)(transform.Position.X - Size.X / 2),
                (int)(transform.Position.Y - Size.Y / 2),
                (int)Size.X,
                (int)Size.Y
            );
        }

        public override bool Intersects(Collider other) {
            return other switch {
                BoxCollider box => Bounds.Intersects(box.Bounds),
                CircleCollider circle => circle.Intersects(this),
                _ => false
            };
        }
        
        public override void ResolveAgainst(BoxCollider box) {
            Rectangle overlap = Rectangle.Intersect(Bounds, box.Bounds);
            if (overlap.Width == 0 || overlap.Height == 0) return;

            Vector2 push = overlap.Width < overlap.Height 
                ? new(Bounds.Center.X < box.Bounds.Center.X ? -overlap.Width : overlap.Width, 0)
                : new(0, Bounds.Center.Y < box.Bounds.Center.Y ? -overlap.Height : overlap.Height);
        }
        
        public override void ResolveCollision(Collider other) => other.ResolveAgainst(this);
        public override void ResolveAgainst(CircleCollider circle) => circle.Intersects(this);
    }

    public sealed class CircleCollider : Collider {
        public float Radius { get; private set; }
        
        public CircleCollider(Entity owner, float radius) : base(owner) => Radius = radius;

        public override void UpdateBounds() {
            Bounds = new(
                (int)(Transform.Position.X - Radius),
                (int)(Transform.Position.Y - Radius),
                (int)(Radius * 2),
                (int)(Radius * 2)
            );
        }
        
        public override bool Intersects(Collider other) => other switch {
            CircleCollider circle => Vector2.Distance(Transform.Position, circle.Transform.Position) < Radius + circle.Radius,
            BoxCollider box => Intersects(box),
            _ => false
        };

        public override void ResolveCollision(Collider other) => other.ResolveAgainst(this);
        
        public override void ResolveAgainst(BoxCollider box) {
            Vector2 closest = new(
                MathHelper.Clamp(Transform.Position.X, box.Bounds.Left, box.Bounds.Right),
                MathHelper.Clamp(Transform.Position.Y, box.Bounds.Top, box.Bounds.Bottom));
            Vector2 diff = Transform.Position - closest;

            float distance = diff.Length();
            if (distance == 0 || distance >= Radius) return;

            Vector2 push = Vector2.Normalize(diff) * (Radius - distance);
            CollisionManager.ApplyResolution(this, box, push);
        }

        public override void ResolveAgainst(CircleCollider circle) {
            Vector2 delta = circle.Transform.Position - Transform.Position;
            float distance = delta.Length();
            float totalRadius = Radius + circle.Radius;
            if (distance == 0 || distance >= totalRadius) return;

            Vector2 push = Vector2.Normalize(delta) * (totalRadius - distance);
            CollisionManager.ApplyResolution(this, circle, -push);
        }
    }
}