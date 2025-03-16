using System;
using CooleGame.Framework;
using CooleGame.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CooleGame;

public class GameManager : Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    
    //Managers
    private readonly InputManager input = new();
    private SceneManager sceneManager;
    public GameManager() {
        Console.WriteLine("Initialized GameManager");
        graphics = new(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        base.Initialize();
        spriteBatch = new(GraphicsDevice);
        sceneManager = new(Content);
        
        InputManager.BindKey(Keys.Escape, Exit);
    }

    protected override void Update(GameTime gameTime) {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        sceneManager.Update(deltaTime);
        InputManager.Update();
        
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Aquamarine);
        
        spriteBatch.Begin();
        sceneManager.Draw(spriteBatch);
        spriteBatch.End();
        
        base.Draw(gameTime);
    }
}