using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneDataPool : DataPoolBase
{
  private Dictionary<string, int> _sceneLookup = new Dictionary<string, int>();
  private Dictionary<string, Scene> _activeScenes = new Dictionary<string, Scene>();

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
      Scene scene = SceneManager.GetSceneByBuildIndex(sceneIndex);
      if (scene.IsValid())
      {
        _activeScenes[sceneName] = scene;
      }
    }
    else
    {
      // Debug.LogError($"Scene '{sceneName}' not found in build settings.");
    }
  }

  public void UnloadScene(string sceneName)
  {
    if (_sceneLookup.ContainsKey(sceneName))
    {
      SceneManager.UnloadSceneAsync(sceneName);
      _activeScenes.Remove(sceneName);
    }
    else
    {
      // Debug.LogError($"Scene '{sceneName}' not found or is not loaded.");
    }
  }

  public Scene? GetScene(string sceneName)
  {
    if (_activeScenes.TryGetValue(sceneName, out Scene scene))
    {
      return scene;
    }
    // Debug.LogWarning($"Scene '{sceneName}' is not currently loaded.");
    return null;
  }

  public void CreateScene(string sceneName)
  {
    if (!_sceneLookup.ContainsKey(sceneName))
    {
      Scene newScene = SceneManager.CreateScene(sceneName);
      _sceneLookup[sceneName] = SceneManager.sceneCountInBuildSettings;
      _activeScenes[sceneName] = newScene;
    }
    else
    {
      // Debug.LogError($"Scene '{sceneName}' already exists.");
    }
  }
}
