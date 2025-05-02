using Microsoft.Xna.Framework;

namespace MonoEngine.Framework;

public sealed class Singleton<T> where T : class, new()
{
    private static readonly Lazy<T> lazy = new(() => new T());
    public static T Instance => lazy.Value;
}