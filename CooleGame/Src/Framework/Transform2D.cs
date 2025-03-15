using Microsoft.Xna.Framework;

namespace CooleGame.Framework
{
    public struct Transform2D {
        public Vector2 Position = default;
        public float Rotation = 0;
        public Vector2 Scale = Vector2.One;
        public Vector2 Origin = default;

        public Transform2D() {
        
        }
    }
}