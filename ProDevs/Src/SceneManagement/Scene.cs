using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using ProDevs.Framework.ECS.Entity;
using ProDevs.Framework.ECS.System;

namespace ProDevs {
    public class Scene {
        public List<Entity> Entities { get; private set; } = new();
        public RenderSystem Renderer { get; private set; } = new();
        
        public Entity CreateEntity(string entityName) {
            Entity entity = new(entityName);
            Entities.Add(entity);
            Renderer.Register(entity);
            return entity;
        }

        public void RemoveEntity(Entity entity) {
            Entities.Remove(entity);
            Renderer.Unregister(entity);
        }

        public void Save(string path) {
            var json = JsonSerializer.Serialize(Entities, new JsonSerializerOptions {
                WriteIndented = true, //For Pretty-printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Avoid serializing null values
            });
            File.WriteAllText(path, json);
        }

        public void Load(string path) {
            var json = File.ReadAllText(path);
            Entities = JsonSerializer.Deserialize<List<Entity>>(json, new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true 
            }) ?? new List<Entity>();  // Fallback to empty list if deserialization fails
        }
    }
}