// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class RoomManager : ManagerBase
// {
//   private int gridWidth;
//   private int gridHeight;
//   private Cell[,] grid;
//   public GameLevel GameLevel { get; private set; }
//
//   public RoomManager(int gridWidth, int gridHeight)
//   {
//     this.gridWidth = gridWidth;
//     this.gridHeight = gridHeight;
//     grid = new Cell[gridWidth, gridHeight];
//     for (int i = 0; i < gridWidth; i++)
//     {
//       for (int j = 0; j < gridHeight; j++)
//       {
//         grid[i, j] = new Cell();
//       }
//     }
//     GameLevel = DungeonGen.Generate();
//   }
//
//   public bool PlaceRoom(Room room)
//   {
//     if (!IsValidPlacement(room))
//     {
//       return false;
//     }
//
//     for (int i = 0; i < room.Height; i++)
//     {
//       for (int j = 0; j < room.Width; j++)
//       {
//         grid[room.X + j, room.Y + i].IsOccupied = true;
//         grid[room.X + j, room.Y + i].OccupyingRoom = room;
//       }
//     }
//
//     GameLevel.Rooms.Add(room);
//     return true;
//   }
//
//   private bool IsValidPlacement(Room room)
//   {
//     if (room.X < 0 || room.Y < 0 || room.X + room.Width > gridWidth || room.Y + room.Height > gridHeight)
//     {
//       return false;
//     }
//
//     for (int i = 0; i < room.Height; i++)
//     {
//       for (int j = 0; j < room.Width; j++)
//       {
//         if (grid[room.X + j, room.Y + i].IsOccupied)
//         {
//           return false;
//         }
//       }
//     }
//
//     return true;
//   }
// }
//
