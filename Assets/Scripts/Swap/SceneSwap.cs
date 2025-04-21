using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwapper : MonoBehaviour
{
    private SaveController savecontroller;
#pragma warning disable 0649 //private variables
  [SerializeField] private string sceneName;
  [SerializeField] private int movX;
  [SerializeField] private int movY;

#pragma warning restore 0649

  private void OnTriggerEnter2D(Collider2D collision)
  {
        savecontroller = FindAnyObjectByType<SaveController>();
    GameManager instance = GameManager.Instance;
    var sceneManager = instance.Get<KOGSceneManager>();
    Player player = collision.gameObject.GetComponent<Player>();
    player.gameObject.transform.position += new Vector3(movX, movY, 0);
        if (player)
            savecontroller.SaveGame();
      sceneManager.LoadScene(sceneName);
  }
}
