using System;
using System.Collections.Generic;

public struct Direction
{
  public Direction(bool n, bool s, bool e, bool w)
  {
    N = n;
    S = s;
    E = e;
    W = w;
  }

  public bool N { get; }
  public bool S { get; }
  public bool E { get; }
  public bool W { get; }
}

public struct Coords
{
  public Coords(double x, double y)
  {
    X = x;
    Y = y;
  }

  public double X { get; }
  public double Y { get; }
}

public class Entryway
{
  public Direction facing = new Direction(false, false, false, false);
  public System.Int32 offset;

  public Entryway(char face, System.Int32 offset)
  {
    this.offset = offset;
  }
}

public class Cell
{
  // Cell should be 5x5
  public static System.Int32 size = 5;
  public bool visited = false;
  public Direction traversibleStatus = new Direction(false, false, false, false);
}
