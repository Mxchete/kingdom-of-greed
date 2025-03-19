using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : ManagerBase
{
  private GameObject roomPrefab;
  public Vector2Int dungeonSize = new Vector2Int(10, 10);
  public Vector2 roomOffset = new Vector2(10, 10);

  public void ReadConfig(DungeonConfig config)
  {
    roomPrefab = Resources.Load<GameObject>(config.roomAssetPath);
    Debug.Log(config.roomAssetPath);
    dungeonSize = config.dungeonSize;
    roomOffset = config.roomOffset;
  }

  public void Create(int seed)
  {
    if (roomPrefab == null)
    {
      Debug.LogError("Room prefab is not assigned!");
      return;
    }

    // Instantiate a new room from the prefab
    // GameObject newRoom = Instantiate(roomPrefab);
    // newRoom.name = "Generated Room";

    // Create a rule object for the main RoomType
    DungeonGenerator.Rule newRule = new DungeonGenerator.Rule
    {
      room = roomPrefab,
      minPosition = new Vector2Int(0, 0),
      maxPosition = dungeonSize,
      obligatory = true
    };

    // Create a new DungeonGenerator object
    GameObject generatorObj = new GameObject("DungeonGenerator");
    DungeonGenerator generator = generatorObj.AddComponent<DungeonGenerator>();

    // Configure the generator
    generator.seed = seed;
    generator.size = dungeonSize;
    generator.offset = roomOffset;
    generator.rooms = new DungeonGenerator.Rule[] { newRule };

    // Generate dungeon
    generator.Generate();
  }
}

