using System.Net;
using System.Text.Json;
namespace MonoEngine.Framework;

public abstract class DataAsset {
    public virtual void Save(string filePath) {
        string json = JsonSerializer.Serialize(this, GetType(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public static T? Load<T>(string filePath) where T : DataAsset {
        string json = File.ReadAllText(filePath);
        return (T)JsonSerializer.Deserialize(json, typeof(T))!;
    }
}