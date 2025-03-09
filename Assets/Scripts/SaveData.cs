using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Start is called before the first frame update
    public Vector3 playerPosition;
    public string mapBoundary;//boundry name either maze or otherBoundry so far
    
    public  List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
}
