
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework.components;

namespace MonoEngine.Entity
{
    public class Player : Framework.Entity
    {
        public Transform Transform { get; private set; }
        public static Collider Collider { get; private set; }
        public PhysicsBody Rgbd { get; private set; } = new(Collider);

        public Player()
        {
            Name = "Player";
            Collider = AddComponent<Collider>();
            AddComponent(Rgbd);
        }
    }
}