using Numerics_Vector2 = System.Numerics.Vector2;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoEngine.Framework;
public class Transform : Component
{
    public Vector2 Position = default;
    public float Rotation = 0;
    public Vector2 Scale = Vector2.One;
    public Vector2 Origin = default;
        
    public static implicit operator Numerics_Vector2(Transform transform) => new(transform.Position.X, transform.Position.Y);
    public static implicit operator Transform(Numerics_Vector2 newPos) => new() { Position = new(newPos.X, newPos.Y) };
        
    public Numerics_Vector2 GetScaleAsNumerics() => new(Scale.X, Scale.Y);
    public void SetScaleAsNumerics(Numerics_Vector2 newScale) => Scale = newScale;
}