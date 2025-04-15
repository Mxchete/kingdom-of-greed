using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void NewGame()
    {
        if (!SceneManager.GetSceneByName("Managers").isLoaded)
        {
            SceneManager.LoadScene("Managers", LoadSceneMode.Additive);
        }
    }
    

  public void QuitGame()
  {
    Application.Quit();
  }
}
