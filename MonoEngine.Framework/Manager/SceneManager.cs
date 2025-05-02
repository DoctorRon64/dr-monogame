using System.Collections.Generic;
using Microsoft.VisualBasic;
namespace MonoEngine.Framework;

public class SceneManager : BaseSingleton<SceneManager>
{
    public List<Framework.Entity> Entities => CurrentActiveScene.Entities;
    public Scene CurrentActiveScene { get; private set; } = null!;
    public void LoadScene(Scene scene) => CurrentActiveScene = scene;
    public void UnLoadScene(Scene scene) => CurrentActiveScene = null!;
    public void AddEntity(Framework.Entity entity) => CurrentActiveScene.Entities.Add(entity);
    public void RemoveEntity(Framework.Entity entity) => CurrentActiveScene.Entities.Remove(entity);
}

public class Scene
{
    public List<Framework.Entity> Entities { get; private set; } = new();
}