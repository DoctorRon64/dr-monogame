using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework;

namespace MonoEngine;

public class MainMenuState : BaseState<GameManager> {
    public override void OnEnter() {
        InputManager.BindKey(Keys.Enter, () => StateMachine.SwitchState<PlayState>());
        Console.WriteLine("Entered MainMenuState");
    }

    public override void OnExit() {
        Console.WriteLine("Exited MainMenuState");
        InputManager.UnbindKey(Keys.Enter);
    }

    public override void OnUpdate(GameTime gameTime) {
        ImGui.Begin("Main Menu");
        ImGui.Text("Press Enter to Play");
        ImGui.End();
    }
}