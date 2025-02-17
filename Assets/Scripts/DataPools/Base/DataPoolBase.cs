using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataPoolBase
{
  protected DataPoolBase()
  {
    if (!GameDataPool.Instance.CanInstantiate())
    {
      throw new InvalidOperationException("Instances of IDataPool implementations can only be created by GameDataPool.");
    }
  }
}
