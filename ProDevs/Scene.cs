using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using MonoEngine.Framework;
using MonoEngine.Framework.Entity;

namespace MonoEngine {
    public class Scene {
        public List<Entity> Entities { get; private set; } = new();
        
        public Entity CreateEntity(string entityName) {
            Entity entity = new(entityName);
            Entities.Add(entity);
            RenderManager.Instance.Register(entity);
            return entity;
        }

        public void RemoveEntity(Entity entity) {
            Entities.Remove(entity);
            RenderManager.Instance.Unregister(entity);
        }

        public void Save(string path) {
            var json = JsonSerializer.Serialize(Entities, new JsonSerializerOptions {
                WriteIndented = true, //For Pretty-printing
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Avoid serializing null values
            });
            File.WriteAllText(path, json);
        }

        public void Load(string path) {
            foreach (Entity t in Entities) RemoveEntity(t);
    
            var json = File.ReadAllText(path);
            Entities = JsonSerializer.Deserialize<List<Entity>>(json, new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true 
            }) ?? new List<Entity>();
        }

    }
}