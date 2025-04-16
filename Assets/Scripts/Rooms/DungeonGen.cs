using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
  void Awake()
  {
    DontDestroyOnLoad(this.gameObject);
    gmInstance = GameManager.Instance;
  }

  void Update()
  {
    if (gmInstance.Get<KOGSceneManager>().GetSceneName() == "Dungeon")
    {
      this.gameObject.SetActive(true);
    }
    else
    {
      this.gameObject.SetActive(false);
    }
  }

  public enum Spawnable
  {
    notSpawnable = 0,
    spawnable = 1,
    requiredSpawn = 2
  }

  public enum spawnConditions
  {
    multipleSpawns = 0,
    spawnOnceNotSpawned = 1,
    spawnOnceSpawned
  }

  public class Cell
  {
    public bool visited = false;
    public bool[] status = new bool[4];
  }

  // This class represents a spawning rule for a given room object
  // this means we can dynamically set certain rooms to spawn
  [System.Serializable]
  public class Rule
  {
    public GameObject room;
    // public GameObject[] entities;
    public Vector2Int minPosition = new Vector2Int(0, 0);
    public Vector2Int maxPosition = new Vector2Int(Int32.MaxValue, Int32.MaxValue);

    public bool obligatory;
    public bool DungeonStart;
    public bool BossRoom;
    public spawnConditions conditions;

    // Bool-ish function to find which rooms can or SHOULD spawn
    public Spawnable ProbabilityOfSpawning(int x, int y, bool[] status)
    {
      // If spawnonce building has spawned, do not spawn again
      if (conditions == spawnConditions.spawnOnceSpawned)
      {
        return Spawnable.notSpawnable;
      }

      // Spawn the start room whenever we get a chance
      if (DungeonStart && !status[(int)Rooms.direction.left])
      {
        DungeonGenerator.weGotStart = true;
        conditions = spawnConditions.spawnOnceSpawned;
        return Spawnable.requiredSpawn;
      }
      else if (DungeonStart)
      {
        return Spawnable.notSpawnable;
      }

      if (BossRoom && !status[(int)Rooms.direction.right])
      {
        DungeonGenerator.weGotEnd = true;
        conditions = spawnConditions.spawnOnceSpawned;
        return Spawnable.requiredSpawn;
      }
      else if (BossRoom)
      {
        return Spawnable.notSpawnable;
      }

      bool inXBound = x >= minPosition.x && x <= maxPosition.x;
      bool inYBound = y >= minPosition.x && y <= maxPosition.x;
      if (inXBound && inYBound)
      {
        return obligatory ? Spawnable.requiredSpawn : Spawnable.spawnable;
      }

      return Spawnable.notSpawnable;
    }

  }

  public Vector2Int size;
  public int startPos = 0;
  public Rule[] rooms;
  public GameObject defaultEntity;
  public Vector2 offset;
  public int seed = 0;
  static private bool weGotStart = false;
  static private bool weGotEnd = false;
  // This should be defined in a config file
  public int minEnemies = 0, maxEnemies = 10;
  private GameManager gmInstance;

  List<Cell> board;

  public void Generate()
  {
    // Very dumb but i forgive if it works
    seed -= 1;
    while (!weGotEnd && !weGotStart)
    {
      seed += 1;
      MazeGenerator();
    }
  }

  void GenerateDungeon()
  {

    for (int i = 0; i < size.x; i++)
    {
      for (int j = 0; j < size.y; j++)
      {
        Cell currentCell = board[(i + j * size.x)];
        if (currentCell.visited)
        {
          int randomRoom = -1;
          List<int> availableRooms = new List<int>();

          for (int k = 0; k < rooms.Length; k++)
          {
            Spawnable p = rooms[k].ProbabilityOfSpawning(i, j, currentCell.status);

            if (p == Spawnable.requiredSpawn)
            {
              randomRoom = k;
              break;
            }
            else if (p == Spawnable.spawnable)
            {
              availableRooms.Add(k);
            }
          }

          if (randomRoom == -1)
          {
            if (availableRooms.Count > 0)
            {
              randomRoom = availableRooms[UnityEngine.Random.Range(0, availableRooms.Count)];
            }
            else
            {
              randomRoom = 0;
            }
          }

          // Position of new Room
          Vector2 pos = new Vector2(i * offset.x, -j * offset.y);
          // Get new Room
          var newRoom = Instantiate(rooms[randomRoom].room, pos,
              Quaternion.identity, transform).GetComponent<Rooms>();
          newRoom.UpdateRoom(currentCell.status);
          newRoom.name += " " + i + "-" + j;
          if (!rooms[randomRoom].DungeonStart)
          {
            SpawnEnemiesInRoom(pos, newRoom.transform);
          }
        }
      }
    }

  }

  void MazeGenerator()
  {
    UnityEngine.Random.InitState(seed);
    board = new List<Cell>();

    for (int i = 0; i < size.x; i++)
    {
      for (int j = 0; j < size.y; j++)
      {
        board.Add(new Cell());
      }
    }

    int currentCell = startPos;

    Stack<int> path = new Stack<int>();

    int k = 0;

    // parameterize maxK?
    while (k < 1000)
    {
      k++;

      board[currentCell].visited = true;

      if (currentCell == board.Count - 1)
      {
        break;
      }

      // Get list of neighbors that we can goto
      List<int> neighbors = CheckNeighbors(currentCell);

      if (neighbors.Count == 0)
      {
        if (path.Count == 0)
        {
          break;
        }
        else
        {
          currentCell = path.Pop();
        }
      }
      else
      {
        path.Push(currentCell);

        int newCell = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];

        if (newCell > currentCell)
        {
          if (newCell - 1 == currentCell)
          {
            board[currentCell].status[2] = true;
            currentCell = newCell;
            board[currentCell].status[3] = true;
          }
          else
          {
            board[currentCell].status[1] = true;
            currentCell = newCell;
            board[currentCell].status[0] = true;
          }
        }
        else
        {
          if (newCell + 1 == currentCell)
          {
            board[currentCell].status[3] = true;
            currentCell = newCell;
            board[currentCell].status[2] = true;
          }
          else
          {
            board[currentCell].status[0] = true;
            currentCell = newCell;
            board[currentCell].status[1] = true;
          }
        }

      }

    }
    GenerateDungeon();
  }

  List<int> CheckNeighbors(int cell)
  {
    List<int> neighbors = new List<int>();

    int neighborUp = cell - size.x;
    int neighborDown = cell + size.x;
    int neighborRight = cell + 1;
    int neighborLeft = cell - 1;

    // Checks for each neighbor
    if (neighborUp >= 0 && !board[neighborUp].visited)
    {
      neighbors.Add(neighborUp);
    }

    if (neighborDown < board.Count && !board[neighborDown].visited)
    {
      neighbors.Add(neighborDown);
    }

    if (neighborRight % size.x != 0 && !board[neighborRight].visited)
    {
      neighbors.Add(neighborRight);
    }

    if (cell % size.x != 0 && !board[neighborLeft].visited)
    {
      neighbors.Add(neighborLeft);
    }

    return neighbors;
  }

  void SpawnEnemiesInRoom(Vector2 roomPosition, Transform roomTransform)
  {
    if (defaultEntity == null)
    {
      Debug.LogError("Enemy prefab not found in Resources!");
      return;
    }
    int numEnemies = WeightedRandom(minEnemies, maxEnemies + 1);
    for (int i = 0; i < numEnemies; i++)
    {
      // This should be taken care of in a config file
      float offsetX = UnityEngine.Random.Range(-10f, 10f);
      float offsetY = UnityEngine.Random.Range(-10f, 10f);

      Vector2 spawnPosition = roomPosition + new Vector2(offsetX, offsetY);

      GameObject enemy = Instantiate(defaultEntity, spawnPosition, Quaternion.identity, roomTransform);
      enemy.name = "Enemy_" + roomTransform.name + "_" + i;
    }
  }

  int WeightedRandom(int min, int max)
  {
    List<int> possibleNum = new List<int>();
    List<float> weights = new List<float>();

    float totalWeight = 0f;

    for (int i = min; i <= max; i++)
    {
      float weight = 1f / (i + 1);
      possibleNum.Add(i);
      weights.Add(weight);
      totalWeight += weight;
    }

    float randomValue = UnityEngine.Random.Range(0f, totalWeight);
    float cumulative = 0f;

    for (int i = 0; i < weights.Count; i++)
    {
      cumulative += weights[i];
      if (randomValue <= cumulative)
      {
        return possibleNum[i];
      }
    }

    return min;
  }
}
