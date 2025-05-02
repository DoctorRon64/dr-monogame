namespace MonoEngine.Framework;

public abstract class BaseSingleton<T> where T : class, new()
{
    private static T? _instance;

    public static T Instance => _instance ??= new T();

    // Optional override for manual creation or injection
    public static void SetInstance(T instance) => _instance = instance;

    // Prevent external instantiation
    protected BaseSingleton() { }
}