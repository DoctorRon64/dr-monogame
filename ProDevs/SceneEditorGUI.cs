using System;
using System.IO;
using System.Linq;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine.Framework;
using MonoEngine.Framework.Entity;
using MonoGame.ImGuiNet;
using Numerics_Vector2 = System.Numerics.Vector2;
using Numerics_Vector4 = System.Numerics.Vector4;
using Vector2 = Microsoft.Xna.Framework.Vector2;

public enum EditorMode {
    Edit,
    Play
}

namespace MonoEngine {
    public class SceneEditorGui: Singleton<SceneEditorGui> {
        private Entity selected;
        private EditorMode currentMode = EditorMode.Edit;
        
        private Scene scene = null!;
        private ImGuiRenderer imGuiRenderer = null!;

        public void Initialize(Scene scene, ImGuiRenderer imGuiRenderer) {
            this.imGuiRenderer = imGuiRenderer;
            this.scene = scene;  
        }

        public void Draw() {
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            
            Toolbar();
            HandleHierachy();
            HandleInspector();
        }

        private void Toolbar() {
            ImGui.SetNextWindowSize(new(800, 400), ImGuiCond.FirstUseEver);
            ImGui.Begin("Toolbar");

            switch (currentMode) {
                case EditorMode.Edit:
                    if (ImGui.Button("▶ Play")) EnterPlayMode();
                    ImGui.SameLine();
                    break;
                case EditorMode.Play:
                    if (ImGui.Button("■ Stop")) ExitPlayMode();
                    ImGui.SameLine();
                    break;
                default:
                    ImGui.Text("Unknown Editor Mode");
                    break;
            }
            
            string ScenePath = Path.Combine("Content", "Scenes", "scene.json");
            if (ImGui.Button("Save")) scene.Save(ScenePath);
            ImGui.SameLine();
            if (ImGui.Button("Load")) scene.Load(ScenePath);
            ImGui.SameLine();
            
            ImGui.End();
        }
        
        private void EnterPlayMode() {
            currentMode = EditorMode.Play;
            Console.WriteLine("Enter Play Mode");
        }

        private void ExitPlayMode() {
            scene = scene;
            currentMode = EditorMode.Edit;
            Console.WriteLine("Exit Play Mode");
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
    
            foreach (Entity entity in scene.Entities.Where(entity => ImGui.Selectable(entity.Name, entity == selected))) {
                selected = entity;
            }

            ImGui.End();
        }
        
        private void CreateNewEntity(string entityName) {
            
            Entity newEntity = scene.CreateEntity(entityName);
            newEntity.AddComponent(new Transform());
            newEntity.AddComponent(new SpriteComponent());
        }
    }
}