using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProDevs.Framework;
using ProDevs.Framework.Components;
using ProDevs.Framework.Objects;
using ProDevs.Managers;

namespace ProDevs;

public class GameManager : Game {
    private GraphicsDeviceManager graphics;
    private Renderer renderer;
    
    //Managers
    private readonly InputManager input = new();
    private Scene scene;

    public GameManager() {
        Console.WriteLine("Initialized GameManager");
        
        graphics = new(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        base.Initialize();
        
        renderer = new Renderer(GraphicsDevice);
        scene = new(Content);
        
        InputManager.BindKey(Keys.Escape, Exit);
    }

    protected override void Update(GameTime gameTime) {
        
        
        
        
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        scene.Update(deltaTime);
        InputManager.Update();
        
        base.Update(gameTime);
    }

    protected override void LoadContent() {
        Texture2D Texture = Content.Load<Texture2D>(@"Textures\game");
        GameObject objecter = new();
    }
    
    
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Aquamarine);
        renderer.Draw();
        
        base.Draw(gameTime);
    }
}