using System;
using UnityEngine;

abstract class GameEntity : MonoBehavior
{
  GameDataPool gameData = GameDataPool.Instance;

  protected abstract void UpdateEntity();

  void Update()
  {
    if (gameData.StateIsRunning())
    {
      UpdateEntity();
    }
  }
}
