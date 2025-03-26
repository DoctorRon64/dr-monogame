using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.Components;
using ProDevs.Framework.Interfaces;

namespace ProDevs.Framework.Objects {
    public class GameObject : IRenderable {
        private Transform transform = new();
        private readonly List<Component> components = new();
        
        /// <summary>
        /// Transform methods
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        public void SetPosition(Vector2 position) => transform.Position = position;
        public void SetRotation(float rotation) => transform.Rotation = rotation;
        public void SetScale(Vector2 scale) => transform.Scale = scale;
        
        //Component
        public void AddComponent(Component component) => components.Add(component);
        public void RemoveComponent(Component component) => components.Remove(component);
        public T GetComponent<T>() where T : Component => components.OfType<T>().FirstOrDefault();

        public void Update(float deltaTime) {
            foreach (Component component in components) {
                component.Update(deltaTime);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch) {
            foreach (Component component in components) {
                //only if a component implements a Irenderable we can inject the spriteBatch and draw it
            }
            
            spriteBatch.Draw(sprite.Texture, transform.Position, null, sprite.Color, transform.Rotation,
                transform.Origin, transform.Scale, SpriteEffects.None, 0);
        }
    }
}