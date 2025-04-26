using System;
using System.IO;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework;
using MonoEngine.Utility;
using MonoGame.ImGuiNet;
using Numerics_Vector2 = System.Numerics.Vector2;
using VectorN3 = System.Numerics.Vector3;
using VectorN4 = System.Numerics.Vector4;

namespace MonoEngine;

public class EditorState : BaseState<GameManager>
{
    private Framework.Entity selectedEntity;
    private string selectedAsset = string.Empty;

    public override void OnEnter()
    {
        AssetRegistry.Refresh(Blackboard.Content);
        
        InputManager.BindKey(Keys.C, MgcbEditorLauncher.OpenMgcbEditor);
        InputManager.BindKey(Keys.R, MgcbEditorLauncher.RebuildContent);
        InputManager.BindKey(Keys.B, () => StateMachine.SwitchState<MainMenuState>());
    }

    public override void OnExit()
    {
        InputManager.UnbindKey(Keys.B);
        InputManager.UnbindKey(Keys.C);
        InputManager.UnbindKey(Keys.R);
    }

    public override void OnUpdate(GameTime gameTime)
    {
        SceneHierachy();
        ShowInspector(selectedEntity);
        ShowAssetPanel(Blackboard.Content, Blackboard.imGuiRenderer); // <- new!
    }

    private void UtilWindowMaker(string title, Numerics_Vector2 defaultSize, Numerics_Vector2? defaultPos = null, float? aspectX = null, float? aspectY = null)
    {
        // Set default window size on first use
        ImGui.SetNextWindowSize(defaultSize, ImGuiCond.FirstUseEver);

        // Set default window position if provided
        if (defaultPos.HasValue)
            ImGui.SetNextWindowPos(defaultPos.Value, ImGuiCond.FirstUseEver);

        // Begin window
        ImGui.Begin(title);

        // Enforce aspect ratio only if both values are provided
        if (aspectX.HasValue && aspectY.HasValue)
        {
            float aspect = aspectX.Value / aspectY.Value;
            var currentSize = ImGui.GetWindowSize();
            float enforcedWidth = currentSize.Y * aspect;

            if (Math.Abs(currentSize.X - enforcedWidth) > 1f)
            {
                // Adjust window size based on aspect ratio
                ImGui.SetWindowSize(new(enforcedWidth, currentSize.Y));
            }
        }
    }
    private void SceneHierachy()
    {
        // Set the default size and position based on desired aspect ratio
        UtilWindowMaker("🧱 Scene Hierarchy", new Numerics_Vector2(300, 200), new Numerics_Vector2(50, 50), 2, 1);

        ImGui.TextDisabled("Entities in scene:");
        ImGui.Separator();

        foreach (Framework.Entity entity in SceneManager.Entities)
        {
            bool isSelected = selectedEntity == entity;
            if (ImGui.Selectable($"{entity.GetEntityName()}##{entity.Id}", isSelected))
            {
                selectedEntity = entity;
            }
        }

        ImGui.End();
    }
    private void ShowInspector(Framework.Entity entity)
    {
        // Create the Inspector Window with a 3:4 aspect ratio
        UtilWindowMaker("🔍 Inspector", new Numerics_Vector2(350, 400), new Numerics_Vector2(400, 100), 3, 4);

        if (entity == null)
        {
            ImGui.TextDisabled("No entity selected.");
            ImGui.End();
            return;
        }

        ImGui.Text($"🆔 ID: {entity.Id}");

        string name = entity.Name;
        if (ImGui.InputText("Name", ref name, 100))
            entity.Name = name;

        // Handling Transform component
        if (entity.TryGetComponent(out Transform transform))
        {
            ImGui.SeparatorText("🧭 Transform");

            var position = transform.PositionNumerics;
            var scale = transform.ScaleNumerics;
            var origin = transform.OriginNumerics;
            float rotation = transform.Rotation;

            ImGui.DragFloat2("Position", ref position, 1.0f, -10000f, 10000f);
            ImGui.DragFloat2("Scale", ref scale, 0.01f, 0.01f, 10.0f);
            ImGui.DragFloat2("Origin", ref origin, 0.01f, -1.0f, 1.0f);
            ImGui.DragFloat("Rotation", ref rotation, 0.5f, -360f, 360f);

            transform.PositionNumerics = position;
            transform.ScaleNumerics = scale;
            transform.OriginNumerics = origin;
            transform.Rotation = rotation;
        }

        // Handling Sprite component
        if (entity.TryGetComponent(out Sprite sprite))
        {
            ImGui.SeparatorText("🖼 Sprite");

            string currentPath = sprite.GetTexturePath();
            if (ImGui.BeginCombo("Texture", string.IsNullOrEmpty(currentPath) ? "<None>" : currentPath))
            {
                foreach (string path in AssetRegistry.SpritePaths)
                {
                    bool isSelected = path == currentPath;
                    if (ImGui.Selectable(path, isSelected))
                    {
                        string assetName = Path.ChangeExtension(path, null);
                        try { sprite.SetTexture(assetName, Blackboard.Content); }
                        catch (Exception ex) { Console.WriteLine($"[Inspector] Failed to load texture: {ex.Message}"); }
                    }
                    if (isSelected) ImGui.SetItemDefaultFocus();
                }
                ImGui.EndCombo();
            }

            Numerics_Vector2 offset = sprite.GetOffsetN();
            if (ImGui.DragFloat2("Offset", ref offset, 1f, -1000, 1000))
            {
                sprite.Offset = new Vector2(offset.X, offset.Y);
            }
        }

        ImGui.End();
    }
    private void ShowAssetPanel(ContentManager content, ImGuiRenderer imguiRenderer)
    {
        // Create the Asset Panel with a 4:3 aspect ratio
        UtilWindowMaker("🎨 Assets", new Numerics_Vector2(200, 300), new Numerics_Vector2(1500, 50), 3, 1);

        int thumbnailSize = 64;
        int padding = 16;
        float cellSize = thumbnailSize + padding;
        float panelWidth = ImGui.GetContentRegionAvail().X;
        int columnCount = Math.Max(1, (int)(panelWidth / cellSize));
        int index = 0;

        foreach (string assetPath in AssetRegistry.SpritePaths)
        {
            IntPtr texturePtr = AssetRegistry.GetThumbnail(assetPath, content, imguiRenderer);

            ImGui.BeginGroup();

            if (texturePtr != IntPtr.Zero)
            {
                ImGui.Image(texturePtr, new Numerics_Vector2(thumbnailSize, thumbnailSize));
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Image(texturePtr, new Numerics_Vector2(thumbnailSize * 2, thumbnailSize * 2));
                    ImGui.Text(assetPath);
                    ImGui.EndTooltip();
                }
            }

            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            ImGui.TextWrapped(fileName);

            ImGui.EndGroup();

            index++;
            if (index % columnCount != 0) ImGui.SameLine();
        }

        ImGui.End();
    }
}