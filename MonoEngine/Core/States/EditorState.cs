using System;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework;
using MonoEngine.Framework.Utility;
using Numerics_Vector2 = System.Numerics.Vector2;
using VectorN3 = System.Numerics.Vector3;
using VectorN4 = System.Numerics.Vector4;

namespace MonoEngine;

public class EditorState : BaseState<GameManager> {
    private Framework.Entity selectedEntity;
    private string selectedAsset = "";
    
    public override void OnEnter() {
        AssetRegistry.Refresh();
        InputManager.BindKey(Keys.B, () => StateMachine.SwitchState<MainMenuState>());
    }

    public override void OnExit() {
        InputManager.UnbindKey(Keys.B);
    }

    public override void OnUpdate(GameTime gameTime) {
        ImGui.Begin("Scene Hierarchy");
        foreach (Framework.Entity entity in SceneManager.Entities) {
            if (ImGui.Selectable($"{entity.GetEntityName()}##{entity.Id}", selectedEntity == entity)) {
                selectedEntity = entity;
            }
        }
        ImGui.End();

        ShowInspector(selectedEntity);
        ShowAssetPanel(); // <- new!
    }
    

    private void ShowAssetPanel() {
        ImGui.Begin("Assets");

        foreach (string assetPath in AssetRegistry.SpritePaths) {
            if (ImGui.Selectable(assetPath)) {
                // Handle selection and set sprite for the selected entity
                Console.WriteLine($"Selected sprite: {assetPath}");
            }
            
            if (ImGui.Selectable(assetPath, selectedAsset == assetPath)) {
                selectedAsset = assetPath;
            }
        }
        ImGui.End();
    }

    private void ShowInspector(Framework.Entity entity) {
        ImGui.Begin("Inspector");

        if (entity == null) {
            ImGui.Text("No entity selected.");
            ImGui.End();
            return;
        }

        ImGui.Text($"Entity ID: {entity.Id}");
        string name = entity.Name;
        ImGui.InputText("Name", ref Unsafe.AsRef(name), 100);

        if (entity.TryGetComponent(out Transform transform)) {
            Numerics_Vector2 position = transform.PositionNumerics;
            Numerics_Vector2 scale = transform.ScaleNumerics;
            Numerics_Vector2 origin = transform.OriginNumerics;
            float rotation = transform.Rotation;

            ImGui.Text("Transform");
            ImGui.DragFloat2("Position", ref position, 1.0f, -10000f, 10000f);
            ImGui.DragFloat2("Scale", ref scale, 0.01f, 0.01f, 10.0f);
            ImGui.DragFloat2("Origin", ref origin, 0.01f, -1.0f, 1.0f);
            ImGui.DragFloat("Rotation", ref rotation, 0.5f, -360f, 360f);

            // Apply changes back
            transform.PositionNumerics = position;
            transform.ScaleNumerics = scale;
            transform.OriginNumerics = origin;
            transform.Rotation = rotation;
        }
        
        if (entity.TryGetComponent(out Sprite sprite)) {
            string currentPath = sprite.GetTexturePath();

            ImGui.Text("Sprite");
            if (ImGui.BeginCombo("Texture", string.IsNullOrEmpty(currentPath) ? "<None>" : currentPath)) {
                foreach (string path in AssetRegistry.SpritePaths) {
                    bool isSelected = path == currentPath;
                    if (ImGui.Selectable(path, isSelected)) {
                        sprite.SetTexture(path, Blackboard.Content);
                    }
                    if (isSelected) ImGui.SetItemDefaultFocus();
                }
                ImGui.EndCombo();
            }

            // Optional: offset / color / effects, etc.
            Numerics_Vector2 offset = sprite.GetOffsetN();
            if (ImGui.DragFloat2("Offset", ref offset, 1f, -1000, 1000)) {
                sprite.SetOffset(new Vector2(offset.X, offset.Y));
            }
        }

        ImGui.End();
    }
}
