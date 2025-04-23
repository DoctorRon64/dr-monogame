using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework;

namespace MonoEngine;

public class PlayState : BaseState<GameManager> {
    public override void OnEnter() {
        InputManager.BindKey(Keys.Enter, () => StateMachine.SwitchState<MainMenuState>());
        Console.WriteLine("Entered PlayState");
    }

    public override void OnExit() {
        Console.WriteLine("Exited PlayState");
        InputManager.UnbindKey(Keys.Enter);
    }

    public override void OnUpdate(GameTime gameTime) {
        ImGui.Begin("Game");
        ImGui.Text("Playing the game...");
        ImGui.End();
        
        
        
    }
}