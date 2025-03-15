using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CooleGame;

public class GameManager : Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private readonly GameObject2D ijsje = new();
    
    public GameManager()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        spriteBatch = new SpriteBatch(GraphicsDevice);
        ijsje.SetTexture(Content.Load<Texture2D>("ijsje"));
        ijsje.SetScale(new Vector2(.1f));
        
        Console.WriteLine(" Initialize Game! 😨😨😨😲😲😲🥳🥳🥳");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            Exit();
        }

        Movelol();
        Console.WriteLine(ijsje.Transform2D.Position);
        
        GraphicsDevice.Clear(Color.Aquamarine);
        ijsje.Update(spriteBatch);
        
        base.Update(gameTime);
    }

    private void Movelol() {
        if (Keyboard.GetState().IsKeyDown(Keys.D)) {
            ijsje.Move(1, 0);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A)) {
            ijsje.Move(-1, 0);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.W)) {
            ijsje.Move(0, -1);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S)) {
            ijsje.Move(0, 1);
        }
    }
}