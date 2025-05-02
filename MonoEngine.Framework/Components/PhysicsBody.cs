using Microsoft.Xna.Framework;

namespace MonoEngine.Framework.components;
public class PhysicsBody : Component
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public required Collider Collider;

    public void Update(float deltaTime)
    {
        Velocity += Acceleration * deltaTime;
        Position += Velocity * deltaTime;
        Collider.Position = Position;
    }
}

public class RigidBody : Component
{
    public Vector2 Acceleration;
    public float Mass = 1f;
}
