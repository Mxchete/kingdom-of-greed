using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  private static GameManager _instance;
  private static bool _canInstantiate;
  private static int? seed = null;

  public static GameManager Instance
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
      GameObject obj = new GameObject("GameManager");
      _instance = obj.AddComponent<GameManager>();
      DontDestroyOnLoad(obj);
    }
  }

  private Dictionary<Type, object> _Managers = new Dictionary<Type, object>();

  public bool CanInstantiate()
  {
    return _canInstantiate;
  }

  public void Register<T>() where T : ManagerBase, new()
  {
    Type type = typeof(T);
    if (!_Managers.ContainsKey(type))
    {
      _canInstantiate = true;
      T instance = new T();
      _canInstantiate = false;
      _Managers[type] = instance;
    }
    else
    {
      Debug.LogWarning($"Data pool of type {type} is already registered.");
    }
  }

  public T Get<T>() where T : ManagerBase
  {
    Type type = typeof(T);
    if (_Managers.TryGetValue(type, out object pool))
    {
      return pool as T;
    }
    Debug.LogWarning($"No data pool of type {type} found.");
    return null;
  }

  public void SetSeed(int? gameSeed)
  {
    seed = gameSeed;
  }

  public int? GetSeed()
  {
    return seed;
  }
}

