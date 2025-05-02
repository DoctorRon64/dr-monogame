using System.Drawing;
using Microsoft.Xna.Framework;

namespace MonoEngine.Framework.components;

public class Collider : Component {
    public Vector2 Position;
    public Vector2 Size;
    public bool IsStatic = false;
    public RectangleF Bounds => new(Position.X, Position.Y, Size.X, Size.Y);
    public bool Intersects(Collider other) => Bounds.IntersectsWith(other.Bounds);
}