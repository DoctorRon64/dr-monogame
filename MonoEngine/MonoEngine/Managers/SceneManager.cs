using System.Collections.Generic;

namespace MonoEngine;

public class SceneManager {
    public static List<Framework.Entity> Entities { get; private set; } = new();

    public static void AddEntity(Framework.Entity entity) => Entities.Add(entity);
    public static void RemoveEntity(Framework.Entity entity) => Entities.Remove(entity);
}