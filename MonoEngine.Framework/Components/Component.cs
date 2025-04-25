using System;
using MonoEngine.Framework;

namespace MonoEngine.Framework {
    public abstract class Component {
        private Entity AttachedEntity { get; set; } = null!;
        public void SetEntity(Entity entity) => AttachedEntity = entity;
        public Entity GetEntity() => AttachedEntity;
    }
}