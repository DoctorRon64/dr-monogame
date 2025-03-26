using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProDevs.Framework;
using ProDevs.Framework.Components;
using ProDevs.Framework.Objects;
using ProDevs.Managers;

namespace ProDevs;

public class Scene {
    private readonly List<GameObject> gameObjects = new();
    private readonly GameObject player = new(true);
    public Scene(ContentManager content) {
        Console.WriteLine("Initializing SceneManager");
        
        player.SetTexture(content.Load<Texture2D>("ijsje"));
        player.SetScale(new(.1f));
        player.SetRotation(0);
        player.SetPosition(new(400, 400));
        player.EnablePhysics();
        
        AddObject(player);
        
        const float speed = 20;
        InputManager.BindGamepadButton(Buttons.DPadLeft, ()=> player.(-1,0, speed));
        InputManager.BindGamepadButton(Buttons.DPadRight, ()=> player.Move(1,0, speed));
        InputManager.BindGamepadButton(Buttons.DPadUp, ()=> player.Move(0,-1, speed));
        InputManager.BindGamepadButton(Buttons.DPadDown, ()=> player.Move(0,1, speed));
        
        InputManager.BindKey(Keys.A, () => player.Move(-1, 0, speed));
        InputManager.BindKey(Keys.W, () => player.Move(0, -1, speed));
        InputManager.BindKey(Keys.S, () => player.Move(0, 1, speed));
        InputManager.BindKey(Keys.D, () => player.Move(1, 0, speed));        
    }

    public void AddObject(GameObject obj) => gameObjects.Add(obj);
    public void RemoveObject(GameObject obj) => gameObjects.Remove(obj);

    public void Update(float deltaTime) {
        foreach (GameObject obj in gameObjects) {
            obj.Update(deltaTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        foreach (GameObject obj in gameObjects) {
            obj.Draw(spriteBatch);
        }
    }
}