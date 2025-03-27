using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;
using ProDevs.Framework.ECS.System;

namespace ProDevs;

public class Scene {
    public readonly GameObject Player = new();
    public readonly RenderSystem Renderer = new();
    
    public Scene(ContentManager Content ) {
        Player.AddComponent(new TransformComponent());
        Player.AddComponent(new SpriteComponent());
        Player.AddComponent(new PhysicsComponent());
        
        Player.GetComponent(out TransformComponent transform);
        transform.SetScale(new(.3f));
        transform.SetRotation(0);
        transform.SetPosition(new(125, 125));
        
        Player.GetComponent(out SpriteComponent sprite);
        sprite.SetTexture("sprites/female", Content);
        
        Renderer.Register(Player);
        
        /*
        const float speed = 20;
        InputManager.BindGamepadButton(Buttons.DPadLeft, ()=> physics.addforce));
        InputManager.BindGamepadButton(Buttons.DPadRight, ()=> Player.Move(1,0, speed));
        InputManager.BindGamepadButton(Buttons.DPadUp, ()=> Player.Move(0,-1, speed));
        InputManager.BindGamepadButton(Buttons.DPadDown, ()=> Player.Move(0,1, speed));
        
        InputManager.BindKey(Keys.A, () => Player.Move(-1, 0, speed));
        InputManager.BindKey(Keys.W, () => Player.Move(0, -1, speed));
        InputManager.BindKey(Keys.S, () => Player.Move(0, 1, speed));
        InputManager.BindKey(Keys.D, () => Player.Move(1, 0, speed));  */      
        Console.WriteLine("Initialed SceneManager");
    }

    public void Draw(SpriteBatch spriteBatch) {
        Renderer.Draw(spriteBatch);
    }
}