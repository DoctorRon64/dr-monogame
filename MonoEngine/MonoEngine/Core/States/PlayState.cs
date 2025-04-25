using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework;

namespace MonoEngine;

public class PlayState : BaseState<GameManager> {
    public override void OnEnter() {
        InputManager.BindKey(Keys.Enter, () => StateMachine.SwitchState<MainMenuState>());
        InputManager.BindKey(Keys.B, () => StateMachine.SwitchState<EditorState>());
    }

    public override void OnExit() {
        InputManager.UnbindKey(Keys.Enter);
        InputManager.UnbindKey(Keys.B);
    }

    public override void OnUpdate(GameTime gameTime) {
        ImGui.Begin("Game");
        ImGui.Text("Playing the game...");
        ImGui.End();
        
        
        
    }
}