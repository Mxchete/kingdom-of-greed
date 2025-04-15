using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KOGSceneManager : ManagerBase
{
  private Dictionary<string, int> _sceneLookup = new Dictionary<string, int>();

  public KOGSceneManager()
  {
    if (!GameManager.Instance.CanInstantiate())
    {
      throw new InvalidOperationException("SceneManager can only be created by GameManager.");
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
      SceneManager.sceneLoaded += OnSceneLoaded;
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

  public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    // Only do this for the newly loaded scene
    SceneManager.sceneLoaded -= OnSceneLoaded;
    // Get the main camera
    Camera cam = null;

    // Search all root GameObjects in the scene
    foreach (GameObject rootObj in scene.GetRootGameObjects())
    {
      // Search in children, including inactive ones
      Camera[] cameras = rootObj.GetComponentsInChildren<Camera>(true);
      foreach (Camera potCam in cameras)
      {
        if (potCam.CompareTag("MainCamera"))
        {
          cam = potCam;
          break;
        }
      }

      if (cam != null)
      {
        break;
      }
    }

    if (cam != null)
    {
      GameObject camObj = cam.gameObject;

      if (!camObj.activeSelf)
      {
        camObj.SetActive(true);
        Debug.Log($"Main Camera '{camObj.name}' was inactive. Now activated.");
      }
      else
      {
        Debug.Log($"Main Camera '{camObj.name}' is already active.");
      }
    }
    else
    {
      Debug.LogWarning("No Camera tagged 'MainCamera' found in the scene!");
    }

        //if (File.Exists(saveLocation))
        //{

        //    SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

        //    player.transform.position = saveData.playerPosition;
        //    Debug.Log("");
        //    //FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
        //    inventoryController.SetInventoryItems(saveData.inventorySaveData);
        //    hotbarController.SetHotbarItems(saveData.hotbarSaveData);


        //}
        //else
        //{

        //    SaveGame();

        //}
    }
}

