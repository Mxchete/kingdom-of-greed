using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : ManagerBase
{
  private GameObject defaultRoomPrefab;
  private GameObject defaultEnemyPrefab;
  // Default Initialization, should be overwritten by config
  public Vector2Int dungeonSize = new Vector2Int(10, 10);
  public Vector2 roomOffset = new Vector2(10, 10);

  public void ReadConfig(DungeonConfig config)
  {
    defaultRoomPrefab = Resources.Load<GameObject>(config.roomAssetPath);
    defaultEnemyPrefab = Resources.Load<GameObject>(config.entityPath);
    dungeonSize = config.dungeonSize;
    roomOffset = config.roomOffset;
  }

  public void Create(int? possibleSeed)
  {
    if (defaultRoomPrefab == null)
    {
      Debug.LogError("Room prefab is not assigned!");
      return;
    }

    int seed = possibleSeed ?? RandomNumberGenerator.GetInt32(Int32.MaxValue);

    // Create a rule object for the main RoomType
    DungeonGenerator.Rule newRule = new DungeonGenerator.Rule
    {
      room = defaultRoomPrefab,
      minPosition = new Vector2Int(0, 0),
      maxPosition = dungeonSize,
      obligatory = true,
      conditions = DungeonGenerator.spawnConditions.multipleSpawns
    };

    // Create a new DungeonGenerator object
    GameObject generatorObj = new GameObject("DungeonGenerator");
    DungeonGenerator generator = generatorObj.AddComponent<DungeonGenerator>();

    // Configure the generator
    generator.seed = seed;
    generator.size = dungeonSize;
    generator.offset = roomOffset;
    generator.rooms = new DungeonGenerator.Rule[] { newRule };
    generator.defaultEntity = defaultEnemyPrefab;

    // Generate dungeon
    generator.Generate();
  }
}

