using Microsoft.Xna.Framework;
using ProDevs.Framework.ECS.Components;

namespace ProDevs.Framework.Interfaces {
    public interface ICollider {
        bool OnCollisionEnter(ICollider other);
        bool OnCollisionStay(ICollider other);
        bool OnCollisionExit(ICollider other);

        bool OnTriggerEnter(ICollider other);
        bool OnTriggerStay(ICollider other);
        bool OnTriggerExit(ICollider other);
    }
}