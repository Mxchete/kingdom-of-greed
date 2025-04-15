using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DungeonInit : MonoBehaviour
{
  GameObject vCam;
  private void Start()
  {
    // vCam = Camera.main.gameObject;
    GameManager instance = GameManager.Instance;

    // Get DungeonManager instance
    var dungeonManager = instance.Get<DungeonManager>();
    int? seed = null;
    dungeonManager.Create(seed);
  }
}

