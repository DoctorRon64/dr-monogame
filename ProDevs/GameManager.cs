using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProDevs.Managers;

namespace ProDevs;

public class GameManager : Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
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
        
        spriteBatch = new(GraphicsDevice);
        scene = new(Content);
        
        InputManager.BindKey(Keys.Escape, Exit);
    }

    protected override void Update(GameTime gameTime) {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        InputManager.Update();
        
        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Aquamarine);
        
        scene.Draw(spriteBatch);
        
        base.Draw(gameTime);
    }
}