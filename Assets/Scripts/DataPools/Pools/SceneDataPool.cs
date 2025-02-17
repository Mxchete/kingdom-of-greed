using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDataPool : DataPoolBase
{
  private Dictionary<string, int> _sceneLookup = new Dictionary<string, int>();

  public SceneDataPool()
  {
    if (!GameDataPool.Instance.CanInstantiate())
    {
      throw new InvalidOperationException("SceneDataPool can only be created by GameDataPool.");
    }
    InitializeScenes();
  }

  private void InitializeScenes()
  {
    for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
    {
      string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
      string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
      _sceneLookup[sceneName] = i;
    }
  }

  public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
  {
    if (_sceneLookup.TryGetValue(sceneName, out int sceneIndex))
    {
      SceneManager.LoadScene(sceneIndex, mode);
    }
    else
    {
      Debug.LogError($"Scene '{sceneName}' not found in build settings.");
    }
  }

  public void UnloadScene(string sceneName)
  {
    if (_sceneLookup.ContainsKey(sceneName))
    {
      SceneManager.UnloadSceneAsync(sceneName);
    }
    else
    {
      Debug.LogError($"Scene '{sceneName}' not found or is not loaded.");
    }
  }
}

