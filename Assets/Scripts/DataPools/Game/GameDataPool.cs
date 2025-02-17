using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataPool : MonoBehaviour
{
  private static GameDataPool _instance;
  private static bool _canInstantiate;

  public static GameDataPool Instance
  {
    get
    {
      if (_instance == null)
      {
        CreateInstance();
      }
      return _instance;
    }
  }

  private static void CreateInstance()
  {
    if (_instance == null)
    {
      GameObject obj = new GameObject("GameDataPool");
      _instance = obj.AddComponent<GameDataPool>();
      DontDestroyOnLoad(obj);
    }
  }

  private Dictionary<Type, object> _dataPools = new Dictionary<Type, object>();

  public bool CanInstantiate()
  {
    return _canInstantiate;
  }

  public void Register<T>() where T : DataPoolBase, new()
  {
    Type type = typeof(T);
    if (!_dataPools.ContainsKey(type))
    {
      _canInstantiate = true;
      T instance = new T();
      _canInstantiate = false;
      _dataPools[type] = instance;
    }
    else
    {
      Debug.LogWarning($"Data pool of type {type} is already registered.");
    }
  }

  public T Get<T>() where T : DataPoolBase
  {
    Type type = typeof(T);
    if (_dataPools.TryGetValue(type, out object pool))
    {
      return pool as T;
    }
    Debug.LogWarning($"No data pool of type {type} found.");
    return null;
  }
}

