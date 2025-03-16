using System.Collections.Generic;
using CooleGame.Framework;
using Microsoft.Xna.Framework.Input;

namespace CooleGame;

public class InputHandler {
    private readonly Dictionary<Keys, HashSet<Signal>> keyBindings = new();
    
    //TODO Add support for gamepad
    
    public void Update() {
        KeyboardState keyboardState = Keyboard.GetState();
        Keys[] pressedKeys = keyboardState.GetPressedKeys();

        foreach (Keys key in pressedKeys) {
            if (!keyBindings.TryGetValue(key, out HashSet<Signal> signalList)) continue;
            foreach (Signal signal in signalList) {
                signal.Invoke();
            }
        }
    }

    public void BindKey(Keys key, SignalBase.SignalDelegate callback) {
        if (!keyBindings.ContainsKey(key)) keyBindings[key] = new();
        keyBindings[key].Add(new(callback));
    }

    public void UnbindKey(Keys key) {
        if (!keyBindings.ContainsKey(key)) return;
        keyBindings.Remove(key);
    }
}