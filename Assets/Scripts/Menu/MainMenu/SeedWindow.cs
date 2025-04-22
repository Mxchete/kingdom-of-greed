using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedWindow : MonoBehaviour
{
  string? seedAsString = null;

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

    if (seedAsString != null)
    {
      instance.SetSeed(seedAsString.GetHashCode());
    }

    Debug.Log("Final seed: " + instance.GetSeed());
    sceneManager.LoadScene("StartRoom");
  }

  public void GetSeed(string playerInput)
  {
    seedAsString = playerInput;
  }
}
