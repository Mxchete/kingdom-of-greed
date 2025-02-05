using System.Collections.Generic;
using UnityEngine;

public class GameDataPool : MonoBehaviour
{
    public static GameDataPool Instance { get; private set; }
    
    private Dictionary<System.Type, DataPoolBase> datapools_ = new Dictionary<System.Type, DataPoolBase>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void register_datapool_<T>() where T : DataPoolBase, new()
    {
        var type = typeof(T);
        if (!datapools_.ContainsKey(type))
        {
            datapools_[type] = T.Instance;
        }
        else
        {
            Debug.LogWarning($"DataPool of type {type} is already registered.");
        }
    }

    public T get_datapool_<T>() where T : DataPoolBase, new()
    {
        var type = typeof(T);
        if (!dataPools.TryGetValue(type, out var pool))
        {
            pool = T.Instance;
            dataPools[type] = pool;
        }
        
        return pool as T;
    }
}
