using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataPoolBase
{
  // should we enforce that Data Pools implement singleton pattern?
  protected DataPoolBase()
  {
    if (!GameDataPool.Instance.CanInstantiate())
    {
      throw new InvalidOperationException("DataPools cannot be instantiated on their own; please register this data pool using the GameDataPool!");
    }
  }
}
