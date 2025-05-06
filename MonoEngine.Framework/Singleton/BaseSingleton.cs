namespace MonoEngine.Framework;

public abstract class BaseSingleton<T> where T : class, new()
{
    private static T? instance;

    public static T Instance => instance ??= new T();

    // Optional override for manual creation or injection
    public static void SetInstance(T _instance) => instance = _instance;

    // Prevent external instantiation
    protected BaseSingleton() { }
}