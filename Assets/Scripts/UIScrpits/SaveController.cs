using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Cinemachine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    private string currentScene;
    private GameObject player;
    //Start is called before the first frame update
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        //inventoryController = FindObjectOfType<InventoryController>();
        //hotbarController = FindObjectOfType<HotbarController>();
        //player = GameObject.FindGameObjectWithTag("Player");
        //LoadGame();
    }


    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            //mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name,
            inventorySaveData = InventoryController.Instance.GetInventoryItems(),
            hotbarSaveData = HotbarController.Instance.GetHotbarItems(),
        };//this is writing a json

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {

        if (File.Exists(saveLocation))
        {

            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            //player.Instance.transform.position = saveData.playerPosition;
            Debug.Log("");
            //FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            InventoryController.Instance.SetInventoryItems(saveData.inventorySaveData);
            HotbarController.Instance.SetHotbarItems(saveData.hotbarSaveData);


        }
        else
        {

            SaveGame();

        }

    }
    //create intialize function to call 
    public void replay()
    {
        PlayerStats.Instance.health = PlayerStats.Instance.maxhealth;
        player.SetActive(true);
        PlayerStats.Instance.health = PlayerStats.Instance.maxhealth;
        //saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        //player = GameObject.FindGameObjectWithTag("Player");
        LoadGame();
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

//new game
//load a should save an empty inventory/hotbar and basically the intialized version of all my data/objects
//scene should be the current scene, inventory empty, hotbar empty, player position empty? or null?.

//load saved data
//load the data from the saved data 
//load the inventory/hotbar, load the last saved scene, and the players last saved position.
//if the player object is or was inactive it should be reactivated and set ot full health.

//order goes load scene, activate player, give max health, set player position, get their inventory and get their hotbar