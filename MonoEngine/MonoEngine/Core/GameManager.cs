using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNET;
using Microsoft.Xna.Framework.Content;
using MonoEngine.Core.States;
using MonoEngine.Framework;
using MonoGame.ImGuiNet;
using MonoEngine.Framework.components;
using MonoEngine.Framework.Manager;
using MonoEngine.Framework.Components;

namespace MonoEngine {
    public class GameManager : Game {
        public ImGuiRenderer ImGuiRenderer { get; private set; }
        private readonly GraphicsDeviceManager graphics;
        private readonly Dictionary<string, IntPtr> textureMap = new();
        private SpriteBatch spriteBatch;
        private StateMachine<GameManager> gameStateManager;
        private Scene gameScene;

        public GameManager() {
            graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        protected override void Initialize() {
            Console.WriteLine("Initialized GameManager");
            gameStateManager = new(this, new MainMenuState());
            gameStateManager.AddState<PlayState>();
            gameStateManager.AddState<EditorState>();

            spriteBatch = new(GraphicsDevice);

            ImGuiRenderer = new(this);
            ImGui.GetIO().Fonts.AddFontDefault();
            ImGuiRenderer.RebuildFontAtlas();

            gameScene = new();
            SceneManager.Instance.LoadScene(gameScene);

            Framework.Entity entity = new Framework.Entity("Player");
            entity.AddComponent(new RigidBody());
            entity.AddComponent(new Velocity());
            entity.AddComponent(new Transform());
            entity.AddComponent(new Sprite());
            SceneManager.Instance.AddEntity(entity);

            InputManager.BindKey(Keys.Escape, Exit);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            ImGuiRenderer.BeginLayout(gameTime); // Start ImGui frame

            //Under here logic!
            InputManager.Update();
            MovementManager.Instance.Update(gameTime); // Apply velocities to transforms
            SpatialPartitionManager.Rebuild(); // Clear & rebuild spatial grid
            CollisionSystem.Instance.Update(gameTime); // Detect and resolve collisions
            PhysicsSystem.Instance.Update(gameTime); // Apply forces, gravity, impulses

            //TODO
            //ANIMATION
            //SOUND
            //AI

            gameStateManager.Update(gameTime);

            ImGuiRenderer.EndLayout();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            Color myColor = new (0xFFFF5733);
            GraphicsDevice.Clear(myColor);

            RenderManager.Instance.Draw(spriteBatch); // Draw your game world

            ImGui.Render(); // Ends ImGui frame
            ImGuiRenderer.EndLayout(); // Draws ImGui UI

            base.Draw(gameTime);
        }
    }
}