using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwapper : MonoBehaviour
{

#pragma warning disable 0649 //private variables
  [SerializeField] private string sceneName;
#pragma warning restore 0649

  private void OnTriggerEnter2D(Collider2D collision)
  {
    GameManager instance = GameManager.Instance;
    var sceneManager = instance.Get<KOGSceneManager>();
    Player player = collision.gameObject.GetComponent<Player>();
    if (player)
      sceneManager.LoadScene(sceneName);
  }
}
