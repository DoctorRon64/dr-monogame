using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine.Framework
{
    public class Camera : Entity
    {
        public Transform Transform { get; private set; }

        public Camera()
        {
            Transform = new Transform();
            AddComponent(Transform);
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(-Transform.Position.X, -Transform.Position.Y, 0);
        }

        public Matrix GetProjectionMatrix(Viewport viewport)
        {
            return Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
        }
    }
}
