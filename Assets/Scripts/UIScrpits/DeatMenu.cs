using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDeathController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DeathMenu;
    private GameObject player;
    void Start()
    {
        DeathMenu.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //FindObjectOfType<Player>() == null
        if (player.activeInHierarchy == false)
        {
            DeathMenu.SetActive(true);
        }
        else
        {
            DeathMenu.SetActive(false);
        }
    }
}
