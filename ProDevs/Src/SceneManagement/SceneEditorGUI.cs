using System.Linq;
using System.Numerics;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;

namespace ProDevs {
    public class SceneEditorGui {
        private readonly Scene scene;
        private Entity selected;
        public SceneEditorGui(Scene scene) => this.scene = scene;

        public void Draw(SpriteBatch spriteBatch) {
            scene.Renderer.Draw(spriteBatch);

            if (selected != null) {
                ImGui.SetNextWindowSize(new Vector2(250, 400), ImGuiCond.FirstUseEver); // Set the window size
                ImGui.Begin("Project Window"); // Begin the window
                ImGui.Text("Assets List");
                ImGui.Button("Load Asset");

                string input = selected.GetEntityName();
                ImGui.InputText("Asset Name", ref input, 64); // Example of input box
                ImGui.End();
            }

            //The Hierachy
            ImGui.SetNextWindowSize(new Vector2(400, 400), ImGuiCond.FirstUseEver);
            ImGui.Begin("Hierarchy");
            foreach (Entity entity in scene.Entities.Where(entity => ImGui.Selectable(entity.Name, entity == selected))) {
                selected = entity;
            }

            ImGui.End();

            //The Inspector
            ImGui.SetNextWindowSize(new Vector2(400, 500), ImGuiCond.FirstUseEver);
            ImGui.Begin("Inspector");
            if (selected != null) {
                
                string input2 = selected.GetEntityName();
                if (ImGui.InputText("Name", ref input2, 64)) selected.SetEntityName(input2);
                ImGui.Text($"ID: {selected.Id}");

                if (selected.HasComponent<TransformComponent>()) {
                    TransformComponent transform = selected.GetComponent<TransformComponent>();

                    Vector2 pos = transform;
                    if (ImGui.DragFloat2("Position", ref pos)) {
                        transform.Position = new(pos.X, pos.Y);
                    }

                    float rot = transform.Rotation;
                    if (ImGui.DragFloat("Rotation", ref rot)) {
                        transform.Rotation = rot;
                    }

                    Vector2 scale = transform.GetScaleAsNumerics();
                    if (ImGui.DragFloat2("Scale", ref scale)) {
                        transform.Scale = new(scale.X, scale.Y);
                    }
                }
            }

            ImGui.End();
        }
    }
}