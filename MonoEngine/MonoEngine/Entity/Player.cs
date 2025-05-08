
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework;
using MonoEngine.Framework.components;

namespace MonoEngine.Entity
{
    public class Player : Framework.Entity
    {
        public Transform Transform { get; private set; } = new();
        public static Collider Collider { get; private set; } = new();
        public PhysicsBody Rgbd { get; private set; } = new(Collider);

        public Player(string Name) : base(Name)
        {
            Collider = AddComponent<Collider>();
            AddComponent(Transform);
            AddComponent(Collider);
            AddComponent(Rgbd);
            AddComponent(new Sprite());

            SceneManager.Instance.AddEntity(this);
        }
    }
}