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
   
   public class SpriteComponent : Component, IRenderable  {
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
   
   public class PhysicsComponent : Component {
      public Vector2 Velocity = Vector2.Zero;
      public Vector2 Acceleration = Vector2.Zero;
      public float Drag = 5f;
   }

   public class RigidBodyComponent : Component {
      
   }

   public class CollisionComponent : Component {
      
   }
}