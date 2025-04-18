﻿using System;
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

namespace MonoEngine {
    public class SceneEditorGui: Singleton<SceneEditorGui> {
        private Entity selected;
        private Scene scene = null!;
        private ImGuiRenderer imGuiRenderer = null!;

        private string sceneName = "scene.json";
        private bool saveMode = true;
        
        public void Initialize(Scene scene, ImGuiRenderer imGuiRenderer) {
            this.imGuiRenderer = imGuiRenderer;
            this.scene = scene;  
        }

        public void Draw() {
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            
            Draw(scene);
            HandleHierachy();
            HandleInspector();
        }
        
        public void Draw(Scene scene) {
            ImGui.Begin("Scene Manager");

            if (ImGui.RadioButton("Save", saveMode)) saveMode = true;
            ImGui.SameLine();
            if (ImGui.RadioButton("Load", !saveMode)) saveMode = false;

            ImGui.InputText("Scene Name", ref sceneName, 256);

            if (saveMode) {
                if (ImGui.Button("Save Scene")) {
                    scene.Save(sceneName);
                }
            } else {
                if (ImGui.Button("Load Scene")) {
                    scene.Load(sceneName);
                }
            }

            ImGui.End();
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
                        
                        
                        SpriteEffects effects = sprite.GetSpriteEffects();
                        ImGui.Text("Sprite Effects:");
                        bool flipH = (effects & SpriteEffects.FlipHorizontally) != 0;
                        bool flipV = (effects & SpriteEffects.FlipVertically) != 0;

                        if (ImGui.Checkbox("Flip Horizontally", ref flipH)) {
                            effects = flipH ? (effects | SpriteEffects.FlipHorizontally) : (effects & ~SpriteEffects.FlipHorizontally);
                        }
                        if (ImGui.Checkbox("Flip Vertically", ref flipV)) {
                            effects = flipV ? (effects | SpriteEffects.FlipVertically) : (effects & ~SpriteEffects.FlipVertically);
                        }
                        sprite.SetSpriteEffects(effects);
                        
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