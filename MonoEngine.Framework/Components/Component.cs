using System;
using MonoEngine.Framework;

namespace MonoEngine.Framework {
    public abstract class Component {
        private Entity.Entity AttachedEntity { get; set; } = null;
        public void SetEntity(Entity.Entity entity) => AttachedEntity = entity;
        public Entity.Entity GetEntity() => AttachedEntity;
    }
}