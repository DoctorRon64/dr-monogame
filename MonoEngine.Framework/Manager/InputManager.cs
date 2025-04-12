using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine.Framework {
    public class InputManager : Singleton<InputManager> {
        private static readonly Dictionary<Keys, HashSet<Signal>> keyBindings = new();
        private static readonly Dictionary<Buttons, HashSet<Signal>> gamepadBindings = new();
    
        ~InputManager() {
            keyBindings.Clear();
            gamepadBindings.Clear();
        }
   
        public static void Update() {
        
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys) {
                if (!keyBindings.TryGetValue(key, out HashSet<Signal> signalList)) continue;
                foreach (Signal signal in signalList) {
                    signal.Invoke();
                }
            }
            
            for (int i = 0; i < GamePad.MaximumGamePadCount; i++) {
                GamePadState gamePadState = GamePad.GetState((PlayerIndex)i);

                if (!gamePadState.IsConnected) continue;
                foreach (Buttons button in Enum.GetValues<Buttons>()) {
                    if (!gamePadState.IsButtonDown(button) ||
                        !gamepadBindings.TryGetValue(button, out HashSet<Signal> buttonSignals)) continue;
                    foreach (Signal signal in buttonSignals) {
                        signal.Invoke();
                    }
                }
            }
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