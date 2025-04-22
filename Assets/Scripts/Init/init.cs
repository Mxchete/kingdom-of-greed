using System;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void LoadGameData()
  {
    GameManager instance = GameManager.Instance;
    Debug.Log(Application.streamingAssetsPath);

    // Initialize SceneManager
    instance.Register<KOGSceneManager>();
    var sceneManager = instance.Get<KOGSceneManager>();

    // Initialize DungeonManager
    instance.Register<DungeonManager>();
    DungeonConfig dungeonConf = BaseConfig.LoadConfigFromJson<DungeonConfig>(
        string.Concat(Application.streamingAssetsPath, "/DungeonConfig.json"));
    var dungeonManager = instance.Get<DungeonManager>();
    dungeonManager.ReadConfig(dungeonConf);

    // Load starting scene
    sceneManager.LoadScene("StartMenu");
  }
}
