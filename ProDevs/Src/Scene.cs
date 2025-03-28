using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;
using ProDevs.Framework.ECS.System;

namespace ProDevs {
    public class Scene {
        private List<GameObject> gameObjects = new();
        public RenderSystem Renderer { get; private set; } = new();
        public CollisionSystem CollisionSystem { get; private set; } = new();
        
        public readonly GameObject Player = new();
        public readonly GameObject Ship = new();

        public Scene(ContentManager content) {
            Ship.AddComponent(new TransformComponent());
            Ship.AddComponent(new SpriteComponent());
            Ship.GetComponent(out TransformComponent transformComponent);
        
            transformComponent.SetScale(new(.05f));
            transformComponent.SetPosition(new(10, 400));

            Ship.GetComponent(out SpriteComponent spriteboat);
            spriteboat.SetTexture("sprites/yacht", content);

            Player.AddComponent(new TransformComponent());
            Player.AddComponent(new SpriteComponent());
            Player.AddComponent(new RigidBodyComponent());
            
            Player.GetComponent(out TransformComponent transform);
            transform.SetScale(new(.3f));
            transform.SetRotation(0);
            transform.SetPosition(new(125, 15));
        
            Player.GetComponent(out SpriteComponent sprite);
            sprite.SetTexture("sprites/female", content);

            Player.AddComponent(new BoxCollider(sprite.GetTextureSize()));
            Ship.AddComponent(new BoxCollider(spriteboat.GetTextureSize()));
            
            Renderer.Register(Player);
            Renderer.Register(Ship);

            gameObjects.Add(Player);
            gameObjects.Add(Ship);
            
            /*
            const float speed = 20;
            InputManager.BindGamepadButton(Buttons.DPadLeft, ()=> physics.addforce));
            InputManager.BindGamepadButton(Buttons.DPadRight, ()=> Player.Move(1,0, speed));
            InputManager.BindGamepadButton(Buttons.DPadUp, ()=> Player.Move(0,-1, speed));
            InputManager.BindGamepadButton(Buttons.DPadDown, ()=> Player.Move(0,1, speed));

            InputManager.BindKey(Keys.A, () => Player.Move(-1, 0, speed));
            InputManager.BindKey(Keys.W, () => Player.Move(0, -1, speed));
            InputManager.BindKey(Keys.S, () => Player.Move(0, 1, speed));
            InputManager.BindKey(Keys.D, () => Player.Move(1, 0, speed));  */
            Console.WriteLine("Initialed SceneManager");
        }

        public void Update(float deltaTime) {
            Player.GetComponent(out RigidBodyComponent rigidBodyComponent);
            Player.GetComponent(out TransformComponent transformComponent);
            rigidBodyComponent.Update(transformComponent, deltaTime);
            
            CollisionSystem.Update(gameObjects);
        }

        public void Draw(SpriteBatch spriteBatch) {
            Renderer.Draw(spriteBatch);
        }
    }
}