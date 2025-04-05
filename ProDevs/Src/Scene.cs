using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProDevs.Framework.ECS.Components;
using ProDevs.Framework.ECS.Entity;
using ProDevs.Framework.ECS.System;

namespace ProDevs {
    public class Scene {
        private List<GameObject> gameObjects = new();
        private CollisionSystem collisionSystem = new();
        public RenderSystem Renderer { get; private set; } = new();
        
        public readonly GameObject Player = new();
        public readonly GameObject Ship = new();

        public Scene(ContentManager content) {
            Ship.AddComponent(new TransformComponent());
            Ship.AddComponent(new SpriteComponent());
            Ship.GetComponent(out TransformComponent transformComponent);
        
            transformComponent.SetScale(new(.05f));
            transformComponent.SetPosition(new(100, 400));

            Ship.GetComponent(out SpriteComponent spriteboat);
            spriteboat.SetTexture("sprites/yacht", content);

            Player.AddComponent(new TransformComponent());
            Player.AddComponent(new SpriteComponent());
            Player.AddComponent(new RigidBodyComponent());
            
            Player.GetComponent(out TransformComponent transform);
            transform.SetScale(new(.3f));
            transform.SetRotation(0);
            transform.SetPosition(new(100, 15));
        
            Player.GetComponent(out SpriteComponent sprite);
            sprite.SetTexture("sprites/man", content);

            Player.AddComponent(new CircleCollider(Player, 9.5f));
            Ship.AddComponent(new CircleCollider(Ship, 12.4f));
            
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
            InputManager.BindKey(Keys.D, () => Player.Move(1, 0, speed));  
            */
            
            Console.WriteLine("Initialed SceneManager");
        }

        public void Update(float deltaTime) {
            foreach (GameObject obj in gameObjects) {
                if (obj.TryGetComponent(out TransformComponent transform) &&
                    obj.TryGetComponent(out RigidBodyComponent rigidBody)) {
                    rigidBody.Update(transform, deltaTime);
                }

                if (!obj.TryGetComponent(out Collider collider)) continue;
                collider.UpdateBounds();
            }
            
            // Build BVH for optimized broad-phase collision detection
            collisionSystem.BuildBvh(gameObjects.SelectMany(go => go.GetComponents<Collider>()).ToList());
           
            // Detect and process collisions
            collisionSystem.CheckCollisions(out List<(Collider, Collider)> collisions);
            foreach ((Collider colA, Collider colB) in collisions) {
                Console.WriteLine($"Collision detected between {colA.Owner.Id} and {colB.Owner.Id}");
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Renderer.Draw(spriteBatch);
        }
    }
}