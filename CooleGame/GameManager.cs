using System;
using CooleGame.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CooleGame;

public class GameManager : Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private readonly GameObject2D player = new();
    private readonly InputHandler input = new();
    public GameManager() {
        Console.WriteLine("Initialized GameManager");
        graphics = new(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {
        base.Initialize();

        spriteBatch = new(GraphicsDevice);
        player.SetTexture(Content.Load<Texture2D>("ijsje"));
        player.SetScale(new(.1f));
        player.SetRotation(0);
        player.SetPosition(new(400, 400));

        float speed = 20;
        
        input.BindKey(Keys.A, () => player.Move(-1, 0, speed));
        input.BindKey(Keys.W, () => player.Move(0, -1, speed));
        input.BindKey(Keys.S, () => player.Move(0, 1, speed));
        input.BindKey(Keys.D, () => player.Move(1, 0, speed));
        
        //input.BindKey(Keys.A, () => player.Move(0, 0));
        input.BindKey(Keys.Escape, Exit);
    }

    protected override void Update(GameTime gameTime) {
        input.Update();
        player.Update(gameTime);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Aquamarine);
        
        spriteBatch.Begin();
        player.Draw(spriteBatch);
        spriteBatch.End();
        
        base.Draw(gameTime);
    }
}