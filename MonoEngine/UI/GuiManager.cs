using Microsoft.Xna.Framework;

namespace MonoEngine {
    public class GuiManager(Framework.Entity entity) {
        private readonly EditorGui editorGui = new(entity);

        public void Update(GameTime gameTime) {
            editorGui.Update(gameTime);
        }
    }
}