using System.Collections.Generic;
using System.Data;
using Microsoft.VisualBasic;
using MonoEngine.Framework.Manager;

namespace MonoEngine.Framework;

public class SceneManager : BaseSingleton<SceneManager> {
    public List<Framework.Entity> Entities => CurrentActiveScene.Entities;
    private Scene CurrentActiveScene { get; set; } = null!;

    public void LoadScene(Scene scene) {
        CurrentActiveScene = scene;
        SpatialPartitionManager.Rebuild(); // Clear & rebuild spatial grid
    }

    public void UnLoadScene(Scene scene) => CurrentActiveScene = null!;

    public void AddEntity(Framework.Entity entity) {
        if (CurrentActiveScene == null) {
            throw new InvalidOperationException("⚠⚠⚠ The scene has not been instantiated!! ⚠⚠⚠");
        }
        CurrentActiveScene.Entities.Add(entity);
    }

    public void RemoveEntity(Framework.Entity entity) => CurrentActiveScene.Entities.Remove(entity);
}

public class Scene {
    public List<Framework.Entity> Entities { get; private set; } = new();
}