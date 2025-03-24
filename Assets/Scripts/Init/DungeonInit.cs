using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInit : MonoBehaviour
{
  private void Start()
  {
    GameManager instance = GameManager.Instance;
    Debug.Log(Application.streamingAssetsPath);

    // Get DungeonManager instance
    var dungeonManager = instance.Get<DungeonManager>();
    int? seed = null;
    dungeonManager.Create(seed);
  }
}

