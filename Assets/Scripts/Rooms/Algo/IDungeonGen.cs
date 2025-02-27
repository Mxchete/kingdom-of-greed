using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeonGen
{
  void Generate();
  IDungeon GetDungeon();
}
