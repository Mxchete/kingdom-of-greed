using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedWindow : MonoBehaviour
{
  private void Awake()
  {
    Hide();
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void Confirm()
  {
    GameManager instance = GameManager.Instance;

    var sceneManager = instance.Get<KOGSceneManager>();

    // sceneManager.LoadScene("StartRoom");
    // sceneManager.LoadScene("Dungeon");
    sceneManager.LoadScene("BossRoomKingDom1");
  }
}
