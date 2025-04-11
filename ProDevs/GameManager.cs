using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProDevs.Managers;
using ImGuiNET;
using MonoGame.ImGuiNet;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;
using ProDevs.Framework.ECS.System;

namespace ProDevs {
    public class GameManager : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly InputManager input = new();
        private RenderSystem renderSystem = new();

        private SceneEditorGui editorGui;
        private ImGuiRenderer imGuiRenderer;
        private Scene scene;

        public GameManager() {
            Console.WriteLine("Initialized GameManager");

            graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }

        protected override void Initialize() {
            spriteBatch = new(GraphicsDevice);
            scene = new();

            imGuiRenderer = new(this);
            ImGui.GetIO().Fonts.AddFontDefault();
            imGuiRenderer.RebuildFontAtlas();
            editorGui = new(scene, imGuiRenderer);

            //Set Entity
            Entity player = scene.CreateEntity("Player");
            player.AddComponent(new TransformComponent());
            player.AddComponent(new SpriteComponent());
            player.GetComponent(out SpriteComponent sprite);
            player.GetComponent(out TransformComponent transform);
            sprite.SetTexture("sprites/man", Content);
            transform.Scale = new(0.5f);
            transform.Rotation = 0;
            transform.Position = new(10, 10);
            transform.Origin = sprite.GetSize() / 2f;

            InputManager.BindKey(Keys.Escape, Exit);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            imGuiRenderer.BeginLayout(gameTime); // Start ImGui frame
            editorGui.Draw(); // Build ImGui UI (this must be AFTER NewFrame)   
            InputManager.Update(); //Logic
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Aquamarine);

            RenderSystem.Instance.Draw(spriteBatch); // Draw your game world
            
            ImGui.Render(); // Ends ImGui frame
            imGuiRenderer.EndLayout(); // Draws ImGui UI

            base.Draw(gameTime);
        }
    }
}