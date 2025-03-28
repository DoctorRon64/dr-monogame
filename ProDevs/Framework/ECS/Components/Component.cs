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
        public Rectangle Bounds { get; protected set; }
        public bool IsTrigger { get; private set; } = false;
        public Signal<GameObject> OnCollisionEnter { get; protected set; } = new();
        public Signal<GameObject> OnCollisionStay { get; protected set; } = new();
        public Signal<GameObject> OnCollisionExit { get; protected set; } = new();

        public void SetTrigger(bool isTrigger) => IsTrigger = isTrigger;
        public abstract void UpdateBounds(TransformComponent transform);
    }

    public class BoxCollider : Collider {
        private readonly Vector2 size;

        public BoxCollider(Vector2 size) => this.size = size;
        public BoxCollider(float x, float y) => this.size = new(x, y);

        public override void UpdateBounds(TransformComponent transform) {
            Bounds = new(
                (int)(transform.Position.X - size.X / 2),
                (int)(transform.Position.Y - size.Y / 2),
                (int)size.X,
                (int)size.Y
            );
        }
    }

    public class AudioComponent : Component { }

    public class AnimationComponent : Component { }
}