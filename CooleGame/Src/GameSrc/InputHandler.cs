using System;
using System.Collections.Generic;
using System.Linq;
using CooleGame.Framework;
using Microsoft.Xna.Framework.Input;

namespace CooleGame
{
    public class InputHandler
    {
        public Alert<Keys> OnKeyPressedAlert { get; private set; } = new();
        public Alert<Keys> OnKeyReleasedAlert { get; private set; } = new();

        private readonly HashSet<Keys> currentlyPressedKeys = [];
        private readonly HashSet<Keys> previouslyPressedKeys = [];

        public InputHandler()
        {
            Console.WriteLine("Initialize InputHandler");
        }

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                if (currentlyPressedKeys.Contains(key)) continue;
                OnKeyPressedAlert.Invoke(key);
                currentlyPressedKeys.Add(key);
            }

            foreach (Keys key in previouslyPressedKeys.Where(key => !keyboardState.IsKeyDown(key)))
            {
                OnKeyReleasedAlert.Invoke(key);
                currentlyPressedKeys.Remove(key);
            }
        }

        public void BindKeyPressed(AlertBase.AlertDelegate<Keys> alertDelegate) =>
            OnKeyPressedAlert.AddListener(alertDelegate);

        public void BindKeyReleased(AlertBase.AlertDelegate<Keys> alertDelegate) =>
            OnKeyReleasedAlert.AddListener(alertDelegate);

        public void UnbindKeyPressed(AlertBase.AlertDelegate<Keys> alertDelegate) =>
            OnKeyPressedAlert.RemoveListener(alertDelegate);

        public void UnbindKeyReleased(AlertBase.AlertDelegate<Keys> alertDelegate) =>
            OnKeyReleasedAlert.RemoveListener(alertDelegate);
    }
}