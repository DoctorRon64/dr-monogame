using Microsoft.Xna.Framework;

namespace MonoEngine.Framework;

public class RigidBodyComponent : Component {
    public float Mass = 1f;
    public Vector2 Velocity = Vector2.Zero;
    public Vector2 Acceleration = Vector2.Zero;
    public float Gravity = 981f;
    public float Drag = 0.98f;
    public bool IsKinematic = false;

    public void ApplyForce(Vector2 force) => Acceleration += force / Mass;

    public void HandleCollision(Entity other) {
        Velocity = Vector2.Zero;
        Acceleration = Vector2.Zero;
    }

    public void Update(Transform transform, float deltaTime) {
        if (IsKinematic) return;

        ApplyForce(new(0, Gravity * Mass));
        Velocity += Acceleration * deltaTime;
        Velocity *= Drag;

        transform.Position += Velocity * deltaTime;
        Acceleration = Vector2.Zero;
    }
}