public abstract class DataPoolBase : MonoBehaviour
{
    private static Dictionary<System.Type, DataPoolBase> instances = new Dictionary<System.Type, DataPoolBase>();

    public static T Instance<T>() where T : DataPoolBase
    {
        var type = typeof(T);
        if (!instances.TryGetValue(type, out var instance))
        {
            var obj = new GameObject(type.Name);
            instance = obj.AddComponent<T>();
            instances[type] = instance;
            DontDestroyOnLoad(obj);
        }
        return instance as T;
    }
}
