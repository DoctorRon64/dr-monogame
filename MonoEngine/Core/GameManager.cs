using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET;
using Microsoft.Xna.Framework.Content;
using MonoEngine.Entity;
using MonoEngine.Framework;
using MonoGame.ImGuiNet;

namespace MonoEngine
{
    public class GameManager : Game
    {
        public ImGuiRenderer imGuiRenderer { get; private set; }
        private readonly GraphicsDeviceManager graphics;
        private readonly Dictionary<string, IntPtr> textureMap = new();
        private SpriteBatch spriteBatch;
        private StateMachine<GameManager> gameStateManager;

        public GameManager()
        {
            graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Console.WriteLine("Initialized GameManager");
            gameStateManager = new(this, new MainMenuState());
            gameStateManager.AddState<PlayState>();
            gameStateManager.AddState<EditorState>();

            spriteBatch = new(GraphicsDevice);

            imGuiRenderer = new(this);
            ImGui.GetIO().Fonts.AddFontDefault();
            imGuiRenderer.RebuildFontAtlas();

            var entity = new Framework.Entity("Player");
            entity.AddComponent(new Transform());
            entity.AddComponent(new Sprite());
            SceneManager.AddEntity(entity);

            InputManager.BindKey(Keys.Escape, Exit);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            imGuiRenderer.BeginLayout(gameTime); // Start ImGui frame

            //Under here logic!
            InputManager.Update();
            gameStateManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            RenderManager.Instance.Draw(spriteBatch); // Draw your game world

            ImGui.Render(); // Ends ImGui frame
            imGuiRenderer.EndLayout(); // Draws ImGui UI

            base.Draw(gameTime);
        }
    }
}