using MonoEngine.Framework.components;
using MonoEngine.Framework.Manager;

namespace MonoEngine.Framework;

public class Entity {
    private static int nextId = 0;
    public int Id { get; private set; }
    public string Name;

    private readonly Dictionary<Type, Component> components = new();

    public Entity(string name = "new Entity") {
        Id = nextId++;
        this.Name = name;
        Console.WriteLine($"Object {Name} created with Id: {Id}");
    }

    public int GetEntityId() => Id;

    /// <summary>
    /// Component Stuff
    /// </summary>
    /// <param name="component"></param>
    /// <typeparam name="T"></typeparam>
    public void AddComponent<T>(T component) where T : Component {
        components[typeof(T)] = component;

        if (component is Sprite && !RenderManager.Instance.GetAllEntities().Contains(this)) {
            RenderManager.Instance.Register(this);
        }
    }

    public T AddComponent<T>() where T : Component, new() {
        components[typeof(T)] = new T();
        return (T)components[typeof(T)];
    }

    public void RemoveComponent<T>() where T : Component => components.Remove(typeof(T));
    public void RemoveComponent<T>(Type component) where T : Component => components.Remove(typeof(T));
    public void GetComponent<T>(out T component) where T : Component => component = (T)components[typeof(T)];
    public T GetComponent<T>() where T : Component => (T)components[typeof(T)];
    public bool HasComponent<T>() where T : Component => components.ContainsKey(typeof(T));
    public bool HasComponent<T>(Type component) where T : Component => components.ContainsKey(component);
    public IEnumerable<Component> GetAllComponents() => components.Values;

    public bool TryGetComponent<T>(out T component) where T : Component {
        foreach (Component storedComponent in components.Values) {
            if (storedComponent is not T matchedComponent) continue;
            component = matchedComponent;
            return true;
        }

        component = null!;
        return false;
    }

    public List<T> GetComponents<T>() where T : Component {
        List<T> result = new();
        foreach (Component component in components.Values) {
            if (component is not T match) continue;
            result.Add(match);
        }

        return result;
    }
}