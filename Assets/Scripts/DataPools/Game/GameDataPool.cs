using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataPool : MonoBehaviour
{
  // pointer to singleton object of game data pool
  private static GameDataPool _instance;
  // used to determine whether we can instantiate an instance of a slave pool
  private static bool _canInstantiate;

  // Public accessor for Data Pool
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

  // Singleton constructor
  private static void CreateInstance()
  {
    if (_instance == null)
    {
      GameObject obj = new GameObject("GameDataPool");
      _instance = obj.AddComponent<GameDataPool>();
      DontDestroyOnLoad(obj);
    }
  }

  // List of registered data pools
  private Dictionary<Type, object> _dataPools = new Dictionary<Type, object>();

  // for slave pools to check whether they can instantiate themselves
  public bool CanInstantiate()
  {
    return _canInstantiate;
  }

  // Register a new data pool with the game
  public void Register<T>() where T : DataPoolBase, new()
  {
    Type type = typeof(T);
    if (!_dataPools.ContainsKey(type))
    {
      // must activate canInstantiate; data pool constructor checks!
      _canInstantiate = true;
      T instance = new T();
      _canInstantiate = false;
      _dataPools[type] = instance;
    }
    else
    {
      // Since data pools are also singletons, we cannot instantiate more than one
      Debug.LogWarning($"Data pool of type {type} is already registered.");
    }
  }

  // Returns data pool of respective type
  public T Get<T>() where T : DataPoolBase
  {
    Type type = typeof(T);
    if (_dataPools.TryGetValue(type, out object pool))
    {
      return pool as T;
    }
    Debug.LogWarning($"No data pool of type {type} found.");
    // should we crash the game instead? Or leave to game to handle null
    return null;
  }
}

