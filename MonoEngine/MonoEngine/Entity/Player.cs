
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework.components;

namespace MonoEngine.Entity
{
    public class Player : Framework.Entity
    {
        public Transform Transform { get; private set; }
        public PhysicsBody rgbd { get; private set; }
        public Collider collider { get; private set; }

        public Player()
        {
            Name = "Player";
            collider = AddComponent<Collider>();
            rgbd.Collider = collider;
            AddComponent(rgbd);
        }
    }
}