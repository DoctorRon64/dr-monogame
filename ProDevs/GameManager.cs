using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProDevs.Managers;
using ImGuiNET;
using ImGuiNET.SampleProgram.XNA;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;

namespace ProDevs {
    public class GameManager : Game {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly InputManager input = new();
    
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
            editorGui = new(scene);
            imGuiRenderer = new(this);
            imGuiRenderer.RebuildFontAtlas();
        
            //Set Entity
            Entity player = scene.CreateEntity("Player");
            player.AddComponent(new TransformComponent());
            player.AddComponent(new RigidBodyComponent());
            player.AddComponent(new SpriteComponent());
            player.GetComponent(out SpriteComponent sprite);
            player.GetComponent(out TransformComponent transform);
            sprite.SetTexture("sprites/man", Content);
            Vector2 newScale = new(sprite.GetSize().X / 2, sprite.GetSize().Y);
            transform.Scale = newScale;
            transform.Rotation = 0;
            transform.Position = new(10, 10);
        
            InputManager.BindKey(Keys.Escape, Exit);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime) {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Rendering
            imGuiRenderer.BeforeLayout(gameTime);
            editorGui.Draw(spriteBatch);
            ImGui.Render();
        
            //Logic
            InputManager.Update();
        
            base.Update(gameTime);
        }
    
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Aquamarine);
        
            imGuiRenderer.AfterLayout();
        
            base.Draw(gameTime);
        }
    }
}