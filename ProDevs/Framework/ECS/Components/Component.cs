using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.ECS.Entity;
using ProDevs.Framework.Interfaces;

namespace ProDevs.Framework.ECS.Components {
    public abstract class Component {
        private GameObject AttachedEntity { get; set; }

        public void SetEntity(GameObject entity) => AttachedEntity = entity;
        public GameObject GetEntity() => AttachedEntity;
    }

    public class TransformComponent : Component {
        public Vector2 Position = default;
        public float Rotation = 0;
        public Vector2 Scale = Vector2.One;
        public Vector2 Origin = default;

        public Vector2 GetPosition() => Position;
        public void SetPosition(Vector2 position) => Position = position;
        public void SetRotation(float rotation) => Rotation = rotation;
        public void SetScale(Vector2 scale) => Scale = scale;
    }

    public class SpriteComponent : Component, IRenderable {
        private Texture2D texture = null;
        private Vector2 spriteOffset = Vector2.Zero;
        private readonly Color color = Color.White;
        private SpriteEffects effects = SpriteEffects.None;

        private int TextureWidth => texture?.Width ?? 0;
        private int TextureHeight => texture?.Height ?? 0;
        
        public void Draw(SpriteBatch spriteBatch) {
            if (texture != null) return;
            spriteBatch.Draw(texture, spriteOffset, color);
        }

        public void SetTexture(string assetName, ContentManager content) {
            Texture2D result = content.Load<Texture2D>(assetName);
            if (result == null) {
                Console.WriteLine("ERROR: Texture not found: " + assetName);
                return;
            }

            texture = result;
        }
        
        public Vector2 GetTextureSize() => new(TextureWidth, TextureHeight);
        public Texture2D GetTexture() => texture;
        public Color GetColor() => color;
        public SpriteEffects GetSpriteEffects() => effects;
        
        public void Offset(Vector2 offset) => spriteOffset = offset;
    }

    public class RigidBodyComponent : Component {
        public float Mass = 1f;
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Acceleration = Vector2.Zero;
        public float Gravity = 981f;
        public float Drag = 0.98f;
        public bool IsKinematic = false;

        public void ApplyForce(Vector2 force) => Acceleration += force / Mass;

        public void HandleCollision(GameObject other) {
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }

        public void Update(TransformComponent transform, float deltaTime) {
            if (IsKinematic) return;

            ApplyForce(new Vector2(0, Gravity * Mass));
            Velocity += Acceleration * deltaTime;
            Velocity *= Drag;

            transform.Position += Velocity * deltaTime;
            Acceleration = Vector2.Zero;
        }
    }
    
    public abstract class Collider : Component {
        public Rectangle Bounds;
        public bool IsTrigger = false;
        public GameObject Owner;

        public Collider(GameObject owner) =>  Owner = owner;

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
            if (other is BoxCollider box) {
                return Bounds.Intersects(box.Bounds);
            }
            return false;
        }
    }

    public sealed class CircleCollider : Collider {
        public float Radius { get; private set; }

        public CircleCollider(GameObject owner, float radius) : base(owner) {
            Radius = radius;
            UpdateBounds();
        }

        public override void UpdateBounds() {
            if (!Owner.TryGetComponent(out TransformComponent transform)) return;
            Bounds = new Rectangle(
                (int)(transform.Position.X - Radius),
                (int)(transform.Position.Y - Radius),
                (int)(Radius * 2),
                (int)(Radius * 2)
            );
        }

        public override bool Intersects(Collider other) {
            switch (other) {
                case CircleCollider circle: {
                    float distance = Vector2.Distance(Owner.GetComponent<TransformComponent>().Position,
                        circle.Owner.GetComponent<TransformComponent>().Position);
                    bool result = distance <= (Radius + circle.Radius);
                    Console.WriteLine($"Checking Circle-Circle Collision: {Owner.Id} vs {circle.Owner.Id} -> {result}");
                    return result;
                }
                case BoxCollider box: {
                    // Circle vs AABB intersection check
                    Vector2 closestPoint = new(
                        MathHelper.Clamp(Owner.GetComponent<TransformComponent>().Position.X, box.Bounds.Left, box.Bounds.Right),
                        MathHelper.Clamp(Owner.GetComponent<TransformComponent>().Position.Y, box.Bounds.Top, box.Bounds.Bottom)
                    );

                    float distance = Vector2.Distance(Owner.GetComponent<TransformComponent>().Position, closestPoint);
                    bool result = distance < Radius;
                    Console.WriteLine($"Checking Circle-Box Collision: {Owner.Id} vs {box.Owner.Id} -> {result}");
                    return result;
                }
                default:
                    return false;
            }
        }
    }


    
}