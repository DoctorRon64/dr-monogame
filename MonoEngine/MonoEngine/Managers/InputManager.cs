using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine.Framework;

namespace MonoEngine {
    public class InputManager : BaseSingleton<InputManager> {
        private static readonly Dictionary<Keys, HashSet<Signal>> keyBindings = new();
        private static readonly Dictionary<Buttons, HashSet<Signal>> gamepadBindings = new();

        private static KeyboardState prevKeyboardState;
        private static readonly GamePadState[] prevGamePadStates = new GamePadState[GamePad.MaximumGamePadCount];

        ~InputManager() {
            keyBindings.Clear();
            gamepadBindings.Clear();
        }

        public static void Update() {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            // Safely iterate over a snapshot of keys
            List<Keys> keys = new(keyBindings.Keys);
            foreach (Keys key in keys.Where(key =>
                         currentKeyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key))) {
                if (!keyBindings.TryGetValue(key, out HashSet<Signal> signals)) continue;
                foreach (Signal signal in signals.ToArray()) {
                    signal.Invoke();
                }
            }

            // Gamepad input
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                GamePadState currentGamePadState = GamePad.GetState((PlayerIndex)i);
                if (!currentGamePadState.IsConnected) continue;

                List<Buttons> buttons = new List<Buttons>(gamepadBindings.Keys);
                foreach (Buttons button in buttons.Where(button =>
                             currentGamePadState.IsButtonDown(button) && !prevGamePadStates[i].IsButtonDown(button))) {
                    if (!gamepadBindings.TryGetValue(button, out HashSet<Signal> signals)) continue;
                    foreach (Signal signal in signals.ToArray()) {
                        signal.Invoke();
                    }
                }

                prevGamePadStates[i] = currentGamePadState;
            }

            prevKeyboardState = currentKeyboardState;
        }

        public static void UnbuindAll(bool keyboard = true, bool gamepad = true) {
            if (keyboard) keyBindings.Clear();
            else if (gamepad) gamepadBindings.Clear();
        }

        public static void BindKey(Keys key, SignalBase.SignalDelegate callback) {
            if (!keyBindings.ContainsKey(key)) keyBindings[key] = new();
            keyBindings[key].Add(new(callback));
        }

        public static void UnbindKey(Keys key) {
            if (!keyBindings.ContainsKey(key)) return;
            keyBindings.Remove(key);
        }

        public static void BindGamepadButton(Buttons button, SignalBase.SignalDelegate callback) {
            if (!gamepadBindings.ContainsKey(button)) gamepadBindings[button] = new();
            gamepadBindings[button].Add(new(callback));
        }

        public static void UnbindGamepadButton(Buttons button) {
            if (!gamepadBindings.ContainsKey(button)) return;
            gamepadBindings.Remove(button);
        }
    }
}