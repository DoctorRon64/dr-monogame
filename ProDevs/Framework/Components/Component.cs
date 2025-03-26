using System;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.Components;
using ProDevs.Framework.Objects;

namespace ProDevs.Framework.Components {
   public abstract class Component {
      public abstract void Update(float deltaTime);
   }
}

GameObject jo = new();

jo.attachComponent();

jo.component