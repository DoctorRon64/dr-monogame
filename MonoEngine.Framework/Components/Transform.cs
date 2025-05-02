using Numerics_Vector2 = System.Numerics.Vector2;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoEngine.Framework.components;
public class Transform : Component
{
    public Vector2 Position = default;
    public float Rotation = 0;
    public Vector2 Scale = Vector2.One;
    public Vector2 Origin = default;

    public Numerics_Vector2 PositionNumerics
    {
        get => new(Position.X, Position.Y);
        set => Position = new Vector2(value.X, value.Y);
    }

    public Numerics_Vector2 ScaleNumerics
    {
        get => new(Scale.X, Scale.Y);
        set => Scale = new Vector2(value.X, value.Y);
    }

    public Numerics_Vector2 OriginNumerics
    {
        get => new(Origin.X, Origin.Y);
        set => Origin = new Vector2(value.X, value.Y);
    }
}