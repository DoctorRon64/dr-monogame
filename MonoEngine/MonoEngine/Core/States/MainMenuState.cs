﻿using System;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Core.States;
using MonoEngine.Framework;

namespace MonoEngine;

public class MainMenuState : BaseState<GameManager> {
    public override void OnEnter() {
        InputManager.BindKey(Keys.Enter, () => StateMachine.SwitchState<PlayState>());
        InputManager.BindKey(Keys.F2, () => StateMachine.SwitchState<EditorState>());
    }

    public override void OnExit() {
        InputManager.UnbindKey(Keys.Enter);
        InputManager.UnbindKey(Keys.F2);
    }

    public override void OnUpdate(GameTime gameTime) {
        ImGui.Begin("Main Menu");
        ImGui.Text("Press Enter to Play");
        ImGui.End();
    }
}