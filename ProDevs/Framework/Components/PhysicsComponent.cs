using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProDevs.Framework.Components {
    public class PhysicsComponent(Transform transform, float dragFactor) : Component {
        public Vector2 Velocity { get; private set; } = Vector2.Zero;
        public Vector2 Acceleration { get; private set; } = Vector2.Zero;
        public float DragFactor { get; private set; } = dragFactor;
        private Transform transform = transform;

        public override void Update(float deltaTime) {
            Velocity += Acceleration * deltaTime;
            Velocity *= 1 - (DragFactor * deltaTime);
            transform.Position += Velocity * deltaTime;
        }

        public void SetVelocity(float x, float y) => Velocity = new Vector2(x, y);
        public void AddForce(Vector2 force) => Acceleration += force;
        public void Stop() => Velocity = Vector2.Zero;
    }
}