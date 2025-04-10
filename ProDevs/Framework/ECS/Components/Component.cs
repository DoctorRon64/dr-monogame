using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content; 
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.Interfaces;
using NVector2 = System.Numerics.Vector2;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ProDevs.Framework.ECS.Components {
    public abstract class Component {
        private Entity.Entity AttachedEntity { get; set; } = null;
        public void SetEntity(Entity.Entity entity) => AttachedEntity = entity;
        public Entity.Entity GetEntity() => AttachedEntity;
    }

    public class TransformComponent : Component
    {
        public Vector2 Position = default;
        public float Rotation = 0;
        public Vector2 Scale = Vector2.One;
        public Vector2 Origin = default;
        
        public static implicit operator NVector2(TransformComponent transform) => new(transform.Position.X, transform.Position.Y);
        public static implicit operator TransformComponent(NVector2 newPos) => new() { Position = new(newPos.X, newPos.Y) };
        
        public NVector2 GetScaleAsNumerics() => new(Scale.X, Scale.Y);
        public void SetScaleAsNumerics(NVector2 newScale) => Scale = newScale;
    }

    public class SpriteComponent : Component, IRenderable {
        private Texture2D texture = null;
        private Vector2 spriteOffset = Vector2.Zero;
        private Color color = Color.White;
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
        
        public Vector2 GetSize() => new(TextureWidth, TextureHeight);
        public NVector2 GetSizeN() => new(TextureWidth, TextureHeight);
        public Texture2D GetTexture() => texture;
        
        public Color GetColor() => color;
        public void SetColor(Color newColor) => color = newColor;
        public SpriteEffects GetSpriteEffects() => effects;
        
        public void SetOffset(Vector2 offset) => spriteOffset = offset;
        public Vector2 GetOffset() => spriteOffset;
        public NVector2 GetOffsetN() => new(spriteOffset.X, spriteOffset.Y);
    }

    public class RigidBodyComponent : Component {
        public float Mass = 1f;
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Acceleration = Vector2.Zero;
        public float Gravity = 981f;
        public float Drag = 0.98f;
        public bool IsKinematic = false;

        public void ApplyForce(Vector2 force) => Acceleration += force / Mass;

        public void HandleCollision(Entity.Entity other) {
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }

        public void Update(TransformComponent transform, float deltaTime) {
            if (IsKinematic) return;

            ApplyForce(new(0, Gravity * Mass));
            Velocity += Acceleration * deltaTime;
            Velocity *= Drag;

            transform.Position += Velocity * deltaTime;
            Acceleration = Vector2.Zero;
        }
    }
}