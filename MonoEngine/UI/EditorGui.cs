using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoEngine.Framework;

using Numerics_Vector2 = System.Numerics.Vector2;
using Numerics_Vector4 = System.Numerics.Vector4;

namespace MonoEngine;

public class EditorGui {
    private Framework.Entity selectedEntity = null;

    public EditorGui(Framework.Entity entity) {
        selectedEntity = entity;
    }
    
    public void Update(GameTime gameTime) {
        ImGui.Begin("Scene Viewer");

        this.selectedEntity = selectedEntity;
        
        foreach (Framework.Entity entity in RenderManager.Instance.GetAllEntities()) {
            string label = $"Entity {entity.GetHashCode()}";

            if (ImGui.Selectable(label, selectedEntity == entity)) {
                selectedEntity = entity;
            }
        }

        ImGui.End();

        if (selectedEntity != null) DrawInspector(selectedEntity);
    }
    
    private void DrawInspector(Framework.Entity entity) {
        ImGui.Begin("Inspector");

        foreach (Component component in entity.GetAllComponents()) {
            Type type = component.GetType();
            ImGui.Text($"Component: {type.Name}");

            switch (component) {
                case Transform transform: {
                    Numerics_Vector2 pos = transform;
                    Numerics_Vector2 scale = transform;
                    float rot = transform.Rotation;

                    if (ImGui.DragFloat2("Position", ref pos)) transform.Position = pos;
                    if (ImGui.DragFloat2("Scale", ref scale)) transform.Scale = scale;
                    if (ImGui.DragFloat("Rotation", ref rot)) transform.Rotation = rot;
                    break;
                }
                case SpriteComponent sprite: {
                    Numerics_Vector4 color = sprite.GetColor().ToVector4().ToNumerics();
                    if (ImGui.ColorEdit4("Color", ref color)) {
                        sprite.SetColor(new Color(color));
                    }
                    break;
                }
            }

            ImGui.Separator();
        }
        ImGui.End();
    }
}