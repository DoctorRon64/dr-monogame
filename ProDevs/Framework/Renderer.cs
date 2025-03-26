using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.Interfaces;

namespace ProDevs.Framework {
    /// <summary>
    /// Handles rendering of all IRenderable objects in the game.
    /// Manages a render queue and draws objects efficiently using a SpriteBatch.
    /// </summary>
    public class Renderer {
        private readonly SpriteBatch spriteBatch;
        private readonly List<IRenderable> renderQueue = new();

        /// <summary>
        /// Initializes the Renderer with a SpriteBatch.
        /// </summary>
        /// <param name="graphics">The GraphicsDevice used to create the SpriteBatch.</param>
        public Renderer(GraphicsDevice graphics) {
            spriteBatch = new SpriteBatch(graphics);
        }

        /// <summary>
        /// Adds a renderable object to the render queue.
        /// </summary>
        /// <param name="irenderable">The renderable object to be drawn.</param>
        public void Register(IRenderable irenderable) => renderQueue.Add(irenderable);

        /// <summary>
        /// Removes a renderable object from the render queue.
        /// </summary>
        /// <param name="irenderable">The renderable object to remove.</param>
        public void Unregister(IRenderable irenderable) => renderQueue.Remove(irenderable);

        /// <summary>
        /// Draws all registered renderable objects.
        /// This method should be called within the Game's Draw method.
        /// </summary>
        public void Draw() {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (IRenderable obj in renderQueue) {
                obj.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}