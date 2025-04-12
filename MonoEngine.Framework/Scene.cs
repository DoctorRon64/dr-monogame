using System.Text.Json;
using System.Text.Json.Serialization;

namespace MonoEngine.Framework {
    public class Scene {
        public List<Entity.Entity> Entities { get; private set; } = new();
        
        public Entity.Entity CreateEntity(string entityName) {
            Entity.Entity entity = new(entityName);
            Entities.Add(entity);
            RenderManager.Instance.Register(entity);
            return entity;
        }

        public void RemoveEntity(Entity.Entity entity) {
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
            var json = File.ReadAllText(path);
            Entities = JsonSerializer.Deserialize<List<Entity.Entity>>(json, new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true 
            }) ?? new List<Entity.Entity>();  // Fallback to empty list if deserialization fails
        }
    }
}