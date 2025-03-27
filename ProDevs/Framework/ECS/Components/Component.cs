using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.Interfaces;

namespace ProDevs.Framework.ECS.Components {
    public abstract class Component { }

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
        public Texture2D Texture = null;
        public Vector2 SpriteOffset = Vector2.Zero;
        public Color Color = Color.White;
        public SpriteEffects Effects = SpriteEffects.None;

        public void Draw(SpriteBatch spriteBatch) {
            if (Texture != null) return;
            spriteBatch.Draw(Texture, SpriteOffset, Color);
        }

        public void SetTexture(string assetName, ContentManager content) {
            Texture2D result = content.Load<Texture2D>(assetName);
            if (result == null) {
                Console.WriteLine("ERROR: Texture not found: " + assetName);
                return;
            }

            Texture = result;
        }

        public void Offset(Vector2 offset) => SpriteOffset = offset;
    }

    public class RigidBodyComponent : Component {
        public float Mass = 1f;
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Acceleration = new Vector2(0, -9.81f);
        public float Drag = 5f;

        public bool IsKinematic = false;

        public void ApplyDrag(float deltaTime) {
            Velocity *= (1 - Drag * deltaTime);
        }
        
        public void ApplyForce(Vector2 force) {
            if (IsKinematic) return;
            Acceleration += force / Mass;
        }
        
        public void Update(TransformComponent transform ,float deltaTime) {
            if (IsKinematic) return;
            Velocity += Acceleration * deltaTime;

            ApplyDrag(deltaTime);

            transform.Position += Velocity * deltaTime;
        }
    }

    public class BoxCollider : Component, ICollider {
        public int CollisionLayerIndex = 0;
        public bool IsTrigger = false;
        public Vector2 Center = Vector2.Zero;
        public Rectangle Bounds;
        
        public bool OnCollisionEnter(ICollider other) {
            return other switch {
                BoxCollider otherBox => Bounds.Intersects(otherBox.Bounds),
                SphereCollider otherSphere => Bounds.Intersects(new Rectangle(
                    (int)(otherSphere.Center.X - otherSphere.Bounds.Radius),
                    (int)(otherSphere.Center.Y - otherSphere.Bounds.Radius), (int)(otherSphere.Bounds.Radius * 2),
                    (int)(otherSphere.Bounds.Radius * 2))) // Box vs Sphere
                ,
                _ => false
            };
        }

        public bool OnCollisionStay(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnCollisionExit(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerEnter(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerStay(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerExit(ICollider other) {
            throw new NotImplementedException();
        }
    }
    
    public class CapsuleCollider : Component, ICollider {
        public int CollisionLayerIndex = 0;
        public bool IsTrigger = false;
        public Vector2 Center = Vector2.Zero;
        public float CapsuleRadius = 1f;
        public float CapsuleHeight = 2f;
        
        public bool OnCollisionEnter(ICollider other) {
            return other switch {
                BoxCollider box => HandleBoxCollider(box),
                SphereCollider sphere => HandleSphereCollider(sphere),
                _ => false
            };
        }

        public bool OnCollisionStay(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnCollisionExit(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerEnter(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerStay(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerExit(ICollider other) {
            throw new NotImplementedException();
        }
    }

    public class SphereCollider : Component, ICollider {
        public int CollisionLayerIndex = 0;
        public bool IsTrigger = false;
        public Vector2 Center = Vector2.Zero;
        public BoundingSphere Bounds;
        
        public bool OnCollisionEnter(ICollider other) {
            return other switch {
                BoxCollider box => box.Bounds.Contains(Center) == ContainmentType.Contains,
                SphereCollider sphere => Bounds.Intersects(sphere.Bounds), // Sphere vs Sphere
                _ => false
            };
        }

        public bool OnCollisionStay(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnCollisionExit(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerEnter(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerStay(ICollider other) {
            throw new NotImplementedException();
        }

        public bool OnTriggerExit(ICollider other) {
            throw new NotImplementedException();
        }
    }

    public class AudioComponent : Component { }

    public class AnimationComponent : Component { }
}