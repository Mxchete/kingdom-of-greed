using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGen : MonoBehaviour
{

  public static System.Int32 RowMax = 0;
  public static System.Int32 ColumnMax = 0;
  public static Coords StartPos;
  public static System.Int64 levelSeed = 0;
  public List<Cell> level;

  public DungeonGen(System.Int32 RowMax, System.Int32 ColMax, System.Int64 Seed)
  {
    this.RowMax = RowMax;
    this.ColumnMax = ColMax;

    this.StartPos = new Coords();
  }

  void Generate()
  {

  }
}
