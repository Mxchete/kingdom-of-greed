using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
  public void NewGame()
  {
    GameManager instance = GameManager.Instance;

    var sceneManager = instance.Get<KOGSceneManager>();

    sceneManager.LoadScene("StartRoom");
  }
}
