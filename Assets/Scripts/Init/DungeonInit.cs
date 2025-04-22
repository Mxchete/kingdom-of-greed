using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DungeonInit : MonoBehaviour
{
  static DungeonManager dungeonManager;
  static bool initDone = false;

  private void Start()
  {
    // DontDestroyOnLoad(this.gameObject);

    // vCam = Camera.main.gameObject;
    GameManager instance = GameManager.Instance;

    if (!initDone)
    {
      // Get DungeonManager instance
      dungeonManager = instance.Get<DungeonManager>();
      int? seed = instance.GetSeed();
      dungeonManager.Create(seed);
      initDone = true;
      Debug.Log("Created dungeonmanager");
    }
    else
    {
      dungeonManager.Load();
    }
  }
}

