using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeon
{
  Dictionary<int, Room>.ValueCollection GetRooms();
  int RoomCount();

  Room Get(int id);

  void Add(Room room);

  void LinkOneWay(Room room1, Room room2);

  void Link(Room room1, Room room2);

  void LinkOneWay(Room room1, Room room2, MZSymbol cond);
  void Link(Room room1, Room room2, MZSymbol cond);
  bool RoomsAreLinked(Room room1, Room room2);

  Room FindStart();
  Room FindBoss();
  Room FindGoal();
  Room FindSwitch();

  AreaRectangle GetExtentBounds();
}
