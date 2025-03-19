using System;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void LoadGameData()
  {
    GameManager instance = GameManager.Instance;

    instance.Register<KOGSceneManager>();

    var sceneManager = instance.Get<KOGSceneManager>();

    sceneManager.LoadScene("StartMenu");
  }
}
