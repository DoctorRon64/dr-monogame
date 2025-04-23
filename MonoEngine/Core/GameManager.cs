using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET;
using MonoEngine.Entity;
using MonoEngine.Framework;
using MonoGame.ImGuiNet;

namespace MonoEngine {
    public class GameManager : Game {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ImGuiRenderer imGuiRenderer;
        
        private Framework.Entity player;
        
        private StateMachine<GameManager> gameStateManager;
        private GuiManager GUI;
        
        public GameManager() {
            graphics = new(this);
            Content.RootDirectory = "Assets";
            IsMouseVisible = true;
            
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        protected override void Initialize() {
            Console.WriteLine("Initialized GameManager");
            gameStateManager = new(this, new MainMenuState());
            gameStateManager.AddState<PlayState>();
                
            spriteBatch = new(GraphicsDevice);
            
            imGuiRenderer = new(this);
            ImGui.GetIO().Fonts.AddFontDefault();
            imGuiRenderer.RebuildFontAtlas();

            GUI = new(player);
            
            InputManager.BindKey(Keys.Escape, Exit);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            imGuiRenderer.BeginLayout(gameTime); // Start ImGui frame
            
            //Under here logic!
            InputManager.Update();
            gameStateManager.Update(gameTime);
            GUI.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Aquamarine);

            RenderManager.Instance.Draw(spriteBatch); // Draw your game world
            
            ImGui.Render(); // Ends ImGui frame
            imGuiRenderer.EndLayout(); // Draws ImGui UI
            
            base.Draw(gameTime);
        }
    }
}