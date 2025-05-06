using System.Drawing;
using Microsoft.Xna.Framework;

namespace MonoEngine.Framework.components;

public class Collider : Component {
    public Vector2 Position = Vector2.Zero;
    public Vector2 Size = Vector2.One;
    public RectangleF Bounds => new(
        (int)Position.X, 
        (int)Position.Y,
        (int)Size.X,
        (int)Size.Y);
    public bool Intersects(Collider other) => Bounds.IntersectsWith(other.Bounds);
}