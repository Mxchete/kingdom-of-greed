using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    // Start is called before the first frame update
    public Vector3 playerPosition;
    public string mapBoundary;//boundry name either maze or otherBoundry so far
    public string currentScene;
    public  List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
}
