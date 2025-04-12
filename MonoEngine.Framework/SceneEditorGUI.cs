using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGuiNet;
using Numerics_Vector2 = System.Numerics.Vector2;
using Numerics_Vector4 = System.Numerics.Vector4;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoEngine.Framework {
    public class SceneEditorGui(Scene scene, ImGuiRenderer imGuiRenderer) {
        private Entity.Entity selected;

        public void Draw() {
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            
            HandleHierachy();
            HandleInspector();
        }

        private void HandleInspector() {
            ImGui.SetNextWindowSize(new(400, 500), ImGuiCond.FirstUseEver);
            ImGui.Begin("Inspector", ImGuiWindowFlags.AlwaysAutoResize);
                
            if (selected != null) {
                string input2 = selected.GetEntityName();
                if (ImGui.InputText("Name", ref input2, 64)) selected.SetEntityName(input2);
                ImGui.Text($"ID: {selected.Id}");

                if (selected.HasComponent<Transform>()) {
                    Transform transform = selected.GetComponent<Transform>();

                    Numerics_Vector2 pos = transform;
                    if (ImGui.DragFloat2("Position", ref pos)) {
                        transform.Position = new(pos.X, pos.Y);
                    }

                    float rot = transform.Rotation;
                    if (ImGui.DragFloat("Rotation", ref rot)) {
                        transform.Rotation = rot;
                    }

                    Numerics_Vector2 scale = transform.GetScaleAsNumerics();
                    if (ImGui.DragFloat2("Scale", ref scale)) {
                        transform.Scale = new(scale.X, scale.Y);
                    }
                }

                if (selected.HasComponent<SpriteComponent>()) {
                    SpriteComponent sprite = selected.GetComponent<SpriteComponent>();
                    Texture2D texture = sprite.GetTexture();

                    if (texture != null) {
                        IntPtr texId = imGuiRenderer.BindTexture(texture);
                        Numerics_Vector2 texSize = new(texture.Width, texture.Height);

                        ImGui.Separator();
                        ImGui.Text("Sprite Preview:");
                        ImGui.Image(texId, texSize * 0.25f);

                        Vector2 size = sprite.GetSizeN();
                        ImGui.Text($"Texture Size: {size.X} x {size.Y}");

                        Color color = sprite.GetColor();
                        Numerics_Vector4 colorVec = new(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
                        ImGui.ColorEdit4("Sprite Color", ref colorVec, ImGuiColorEditFlags.NoInputs); // Visual only

                        Color newcolor = new((byte)(colorVec.X * 255), (byte)(colorVec.Y * 255), (byte)(colorVec.Z * 255), (byte)(colorVec.W * 255));
                        sprite.SetColor(newcolor);

                        // SpriteEffects info
                        SpriteEffects effects = sprite.GetSpriteEffects();
                        ImGui.Text($"Sprite Effects: {effects}");

                        Numerics_Vector2 offset = sprite.GetOffsetN();
                        if (ImGui.DragFloat2("Offset", ref offset)) {
                            sprite.SetOffset(offset);
                        }
                    } else {
                        ImGui.Text("No texture assigned.");
                    }
                }
            }

            ImGui.End();
        }

        private void HandleHierachy() {
            ImGui.SetNextWindowSize(new(400, 500), ImGuiCond.FirstUseEver);
            ImGui.Begin("Inspector", ImGuiWindowFlags.NoCollapse);
    
            string entityName = "New Entity";
    
            ImGui.Begin("Hierarchy");
            if (ImGui.InputText("Entity Name", ref entityName, 64)) {
                // Handle any updates to the entity name here if needed
            }
    
            if (ImGui.Button("Create New Entity")) {
                CreateNewEntity(entityName);
            }
    
            foreach (Entity.Entity entity in scene.Entities.Where(entity => ImGui.Selectable(entity.Name, entity == selected))) {
                selected = entity;
            }

            ImGui.End();
        }
        
        private void CreateNewEntity(string entityName) {
            
            Entity.Entity newEntity = scene.CreateEntity(entityName);
            newEntity.AddComponent(new Transform());
            newEntity.AddComponent(new SpriteComponent());
        }
    }
}