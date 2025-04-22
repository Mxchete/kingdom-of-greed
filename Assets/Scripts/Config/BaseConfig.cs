using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public abstract class BaseConfig
{
  public static T LoadConfigFromJson<T>(string filePath) where T : BaseConfig, new()
  {
    if (!File.Exists(filePath))
    {
      Debug.LogError("Config file not found: " + filePath);
      return new T();
    }

    string json = File.ReadAllText(filePath);
    T config = JsonUtility.FromJson<T>(json);
    return config;
  }
}
