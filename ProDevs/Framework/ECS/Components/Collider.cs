using System;
using Microsoft.Xna.Framework;
using ProDevs.Framework.ECS.Entity;

namespace ProDevs.Framework.ECS.Components {
    public abstract class Collider(GameObject owner) : Component {
        public Rectangle Bounds { get; protected set; }
        public bool IsTrigger = false;
        public GameObject Owner { get; private set; } = owner;
        public TransformComponent Transform => Owner?.GetComponent<TransformComponent>();
        public abstract void UpdateBounds();
        public abstract bool Intersects(Collider other);
    }
    
    public sealed class BoxCollider : Collider {
        public Vector2 Size { get; private set; }
    
        public BoxCollider(GameObject owner, Vector2 size) : base(owner) {
            Size = size;
            UpdateBounds();
        }

        public override void UpdateBounds() {
            if (!Owner.TryGetComponent(out TransformComponent transform)) return;
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
    }

    public sealed class CircleCollider : Collider {
        public float Radius { get; private set; }
        
        public CircleCollider(GameObject owner, float radius) : base(owner) => Radius = radius;

        public override void UpdateBounds() {
            Bounds = new(
                (int)(Transform.Position.X - Radius),
                (int)(Transform.Position.Y - Radius),
                (int)(Radius * 2),
                (int)(Radius * 2)
            );
        }
        
        public override bool Intersects(Collider other) {
            switch (other) {
                case CircleCollider circle: {
                    float dx = Transform.Position.X - circle.Transform.Position.X;
                    float dy = Transform.Position.Y - circle.Transform.Position.Y;
                    float distance = MathF.Sqrt(dx * dx + dy * dy);
                    return distance < (this.Radius + circle.Radius);
                }

                case BoxCollider box: {
                    Vector2 closestPoint = new(
                        MathHelper.Clamp(Transform.Position.X, box.Bounds.Left, box.Bounds.Right),
                        MathHelper.Clamp(Transform.Position.Y, box.Bounds.Top, box.Bounds.Bottom)
                    );

                    float distance = Vector2.Distance(Transform.Position, closestPoint);
                    return distance < Radius;
                }

                default:
                    return false;
            }
        }
    }
}