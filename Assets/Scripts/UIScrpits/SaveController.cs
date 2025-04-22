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
    private GameObject inventoryPanel;

    //void Awake()
    //{
    //    inventoryController = FindObjectOfType<InventoryController>();
    //    hotbarController = FindObjectOfType<HotbarController>();
    //}

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        //inventoryController = FindObjectOfType<InventoryController>();
        inventoryController = GetComponent<InventoryController>();
        //if (inventoryController == null)
        //    Debug.LogError("InventoryController is NULL — not found on this GameObject!");
        //else
        //    Debug.Log("InventoryController found!");

        hotbarController = FindObjectOfType<HotbarController>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentScene = SceneManager.GetActiveScene().name;
        //SaveGame();
        //player = GameObject.FindGameObjectWithTag("Player");
        LoadGame();
    }
    //void OnLoad(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.name != "StartMenu")
    //    {
    //    saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

    //        LoadGame();
    //    }
    //}


    public void SaveGame()
    {
        if (string.IsNullOrEmpty(saveLocation))
        {
            saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
            Debug.LogWarning("Save location was uninitialized. Resetting it now.");
        }
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        //if (inventoryController == null)
        //    inventoryController = FindObjectOfType<InventoryController>();

        //if (hotbarController == null)
        //    hotbarController = FindObjectOfType<HotbarController>();

        if (inventoryController == null)
        {
            Debug.LogWarning("SaveGame aborted: Missing required references.");
            return;
        }


        SaveData saveData = new SaveData
        {
            //playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            //mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name,
            currentScene = SceneManager.GetActiveScene().name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),
            
        };//this is writing a json
        Debug.Log("Savedata");
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {

        //if (inventoryController == null)
        //{
        //    Debug.LogError("InventoryController is NULL!");
        //    return;
        //}
        
        //KOGSceneManager.LoadScene(currentScene, LoadSceneMode.Single);
        if (File.Exists(saveLocation))
        {

            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            if (saveData.inventorySaveData == null)
            {
                Debug.LogError("inventorySaveData is NULL!");
                return;
            }
            //player.transform.position = saveData.playerPosition;
            //Debug.Log("Load");
            //FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);

            Debug.Log("Load");
        }
        else
        {

            SaveGame();

        }

    }
    //create intialize function to call 
    public void replay()
    {
        //PlayerStats.Instance.health = PlayerStats.Instance.maxhealth;
        //player.SetActive(true);
        //PlayerStats.Instance.health = PlayerStats.Instance.maxhealth;
        //saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        //player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
    }

    public void loadMainMenu()
    {
        if(player != null)
        {
            // SaveGame();
        }
        
//	GameManager instance = GameManager.Instance;
//	var dungeonManager = instance.Get<DungeonManager>();
//	dungeonManager.DestroyMe();

//        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
        Application.Quit();
    }

    //    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    // Try to find the new inventory panel in the new scene
    //    inventoryPanel = GameObject.Find("InventoryPanel");

    //    if (inventoryPanel == null)
    //    {
    //        Debug.LogWarning("InventoryPanel not found in scene: " + scene.name);
    //    }
    //    else
    //    {
    //        Debug.Log("InventoryPanel linked from GameManager in: " + scene.name);
    //    }
    //}
}

//new game
//load a should save an empty inventory/hotbar and basically the intialized version of all my data/objects
//scene should be the current scene, inventory empty, hotbar empty, player position empty? or null?.

//load saved data
//load the data from the saved data 
//load the inventory/hotbar, load the last saved scene, and the players last saved position.
//if the player object is or was inactive it should be reactivated and set ot full health.

//order goes load scene, activate player, give max health, set player position, get their inventory and get their hotbar
