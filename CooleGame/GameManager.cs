using System;
using CooleGame.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CooleGame
{
    public class GameManager : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
    
        private readonly Alert playerJumpAlert = new();
        private readonly AlertMonitor scoreMonitor = new();
        
        private readonly GameObject2D player = new();
        private readonly InputHandler inputHandler = new();
    
        public GameManager()
        {
            Console.WriteLine("Initialized GameManager");
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        
            spriteBatch = new(GraphicsDevice);
            player.SetTexture(Content.Load<Texture2D>("ijsje"));
            
            player.SetScale(new Vector2(.1f));
            player.SetRotation(90);

            scoreMonitor.AddListener(playerJumpAlert, () => Keyboard.GetState().IsKeyDown(Keys.W));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            inputHandler.Update();
        
            GraphicsDevice.Clear(Color.Aquamarine);
            spriteBatch.Begin();
            player.Update(gameTime);
            player.Draw(spriteBatch);
            spriteBatch.End();
        
            base.Update(gameTime);
        }
    }
}