using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class DungeonConfig : BaseConfig
{
  public Vector2Int dungeonSize;
  public Vector2 roomOffset;
  public string roomAssetPath;
  public string startRoomPath;
}
